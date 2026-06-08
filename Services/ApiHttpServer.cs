using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataApiServiceForm.Models;
using Newtonsoft.Json;

namespace DataApiServiceForm.Services
{
    public class ApiHttpServer : IDisposable
    {
        private readonly int _port;
        private readonly Func<string> _connectionStringProvider;
        private readonly Action<string> _logCallback;
        private readonly OracleQueryExecutor _executor;
        private readonly IndicatorService _indicatorService;

        private HttpListener _listener;
        private CancellationTokenSource _cts;
        private volatile bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public ApiHttpServer(int port, Func<string> connectionStringProvider, Action<string> logCallback)
        {
            _port = port;
            _connectionStringProvider = connectionStringProvider;
            _logCallback = logCallback;
            _executor = new OracleQueryExecutor();
            _indicatorService = new IndicatorService(_connectionStringProvider);
        }

        public void Start()
        {
            if (_isRunning) return;

            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add(string.Format("http://+:{0}/", _port));
                _listener.Start();

                _cts = new CancellationTokenSource();
                _isRunning = true;

                // Start listen loop on a background thread
                var listenThread = new Thread(ListenLoop)
                {
                    IsBackground = true,
                    Name = "ApiHttpServer-Listener"
                };
                listenThread.Start();

                Log(string.Format("API server started on port {0}", _port));
            }
            catch (HttpListenerException ex)
            {
                _isRunning = false;
                if (_listener != null) { _listener.Close(); }
                _listener = null;
                throw new InvalidOperationException(
                    string.Format("Failed to start HTTP listener on port {0}. ", _port) +
                    "You may need to run the following command as Administrator:\n" +
                    string.Format("netsh http add urlacl url=http://+:{0}/ user=Everyone\n", _port) +
                    string.Format("Error: {0}", ex.Message), ex);
            }
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            if (_cts != null) { _cts.Cancel(); }

            try
            {
                if (_listener != null) { _listener.Stop(); }
            }
            catch (ObjectDisposedException) { }
            catch (Exception) { }

            Log("API server stopped");
        }

        private void ListenLoop()
        {
            while (_isRunning)
            {
                try
                {
                    var context = _listener.GetContext();
                    // Dispatch request handling to a thread pool thread
                    Task.Run(() => HandleRequest(context));
                }
                catch (HttpListenerException)
                {
                    // Listener was stopped
                    break;
                }
                catch (ObjectDisposedException)
                {
                    // Listener was disposed
                    break;
                }
            }
        }

        private void HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            try
            {
                // Set CORS headers on all responses
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                // Handle OPTIONS preflight
                if (request.HttpMethod == "OPTIONS")
                {
                    response.StatusCode = 200;
                    response.Close();
                    return;
                }

                var path = request.Url.AbsolutePath;

                // Route: POST /api/query
                if (path.Equals("/api/query", StringComparison.OrdinalIgnoreCase))
                {
                    HandleQuery(context);
                    return;
                }

                // Route: GET /api/indicator/list
                if (path.Equals("/api/indicator/list", StringComparison.OrdinalIgnoreCase) && request.HttpMethod == "GET")
                {
                    HandleIndicatorList(response);
                    return;
                }

                // Route: GET /api/indicator/{code}
                if (path.StartsWith("/api/indicator/", StringComparison.OrdinalIgnoreCase))
                {
                    var remaining = path.Substring("/api/indicator/".Length);

                    // Skip list route (already handled)
                    if (remaining.Equals("list", StringComparison.OrdinalIgnoreCase))
                    {
                        SendIndicatorJsonResponse(response, 405, IndicatorResult.Fail("Method not allowed"));
                        return;
                    }

                    // POST /api/indicator/{code}/execute
                    if (remaining.EndsWith("/execute", StringComparison.OrdinalIgnoreCase) && request.HttpMethod == "POST")
                    {
                        var code = remaining.Substring(0, remaining.Length - "/execute".Length);
                        HandleIndicatorExecute(context, code);
                        return;
                    }

                    // GET /api/indicator/{code}
                    if (request.HttpMethod == "GET" && !remaining.Contains("/"))
                    {
                        HandleIndicatorGet(response, remaining);
                        return;
                    }

                    SendIndicatorJsonResponse(response, 405, IndicatorResult.Fail("Method not allowed"));
                    return;
                }

                // No route matched
                SendJsonResponse(response, 404, QueryResponse.Fail("Not found"));
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                Log(string.Format("Oracle error: {0}", ex.Message));
                SendJsonResponse(response, 500, QueryResponse.Fail(string.Format("Oracle error: {0}", ex.Message)));
            }
            catch (Exception ex)
            {
                Log(string.Format("Internal error: {0}", ex.Message));
                SendJsonResponse(response, 500, QueryResponse.Fail(string.Format("Internal server error: {0}", ex.Message)));
            }
            finally
            {
                try { response.Close(); } catch { }
            }
        }

        private void HandleQuery(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (request.HttpMethod != "POST")
            {
                SendJsonResponse(response, 405, QueryResponse.Fail("Method not allowed. Use POST"));
                return;
            }

            // Read request body
            string body;
            var encoding = request.ContentEncoding ?? Encoding.UTF8;
            using (var reader = new StreamReader(request.InputStream, encoding))
            {
                body = reader.ReadToEnd();
            }

            // Parse JSON
            QueryRequest queryRequest;
            try
            {
                queryRequest = JsonConvert.DeserializeObject<QueryRequest>(body);
            }
            catch (JsonException ex)
            {
                SendJsonResponse(response, 400, QueryResponse.Fail(string.Format("Invalid JSON: {0}", ex.Message)));
                return;
            }

            // Validate sql field
            if (queryRequest == null || string.IsNullOrWhiteSpace(queryRequest.Sql))
            {
                SendJsonResponse(response, 400, QueryResponse.Fail("Missing required field: sql"));
                return;
            }

            // Get connection string
            var connectionString = _connectionStringProvider();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                SendJsonResponse(response, 500, QueryResponse.Fail("Connection string is not configured"));
                return;
            }

            // Execute query
            Log(string.Format("Executing query: {0}", Truncate(queryRequest.Sql, 200)));
            List<string> columns;
            var data = _executor.ExecuteQuery(connectionString, queryRequest.Sql, out columns);

            Log(string.Format("Query completed: {0} row(s) returned", data.Count));
            SendJsonResponse(response, 200, QueryResponse.Ok(data, columns));
        }

        private void HandleIndicatorList(HttpListenerResponse response)
        {
            Log("GET /api/indicator/list");
            var indicators = _indicatorService.GetAllIndicators();

            var data = new List<Dictionary<string, object>>();
            foreach (var ind in indicators)
            {
                data.Add(new Dictionary<string, object>
                {
                    { "indicatorCode", ind.IndicatorCode },
                    { "indicatorName", ind.IndicatorName },
                    { "categoryName", ind.CategoryName },
                    { "dataType", ind.DataType },
                    { "unit", ind.Unit },
                    { "description", ind.Description }
                });
            }

            SendIndicatorJsonResponse(response, 200, IndicatorResult.Ok(data));
        }

        private void HandleIndicatorGet(HttpListenerResponse response, string code)
        {
            Log(string.Format("GET /api/indicator/{0}", code));
            var indicator = _indicatorService.GetIndicatorByCode(code);

            if (indicator == null)
            {
                SendIndicatorJsonResponse(response, 404, IndicatorResult.Fail(string.Format("Indicator not found: {0}", code)));
                return;
            }

            var data = new Dictionary<string, object>
            {
                { "indicatorCode", indicator.IndicatorCode },
                { "indicatorName", indicator.IndicatorName },
                { "categoryName", indicator.CategoryName },
                { "querySql", indicator.QuerySql },
                { "paramDef", indicator.ParamDef },
                { "dataType", indicator.DataType },
                { "unit", indicator.Unit },
                { "description", indicator.Description },
                { "status", indicator.Status }
            };

            SendIndicatorJsonResponse(response, 200, IndicatorResult.Ok(data));
        }

        private void HandleIndicatorExecute(HttpListenerContext context, string code)
        {
            Log(string.Format("POST /api/indicator/{0}/execute", code));

            // Read request body to get parameters
            string body;
            var encoding = context.Request.ContentEncoding ?? Encoding.UTF8;
            using (var reader = new StreamReader(context.Request.InputStream, encoding))
            {
                body = reader.ReadToEnd();
            }

            Dictionary<string, object> parameters = null;
            if (!string.IsNullOrWhiteSpace(body))
            {
                try
                {
                    var requestObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                    if (requestObj != null && requestObj.ContainsKey("parameters"))
                    {
                        var paramsObj = requestObj["parameters"];
                        if (paramsObj is Newtonsoft.Json.Linq.JObject jObj)
                        {
                            parameters = new Dictionary<string, object>();
                            foreach (var prop in jObj.Properties())
                            {
                                parameters[prop.Name] = prop.Value?.ToString();
                            }
                        }
                    }
                }
                catch { }
            }

            var result = _indicatorService.ExecuteIndicator(code, parameters);
            var statusCode = result.Success ? 200 : (result.Error != null && result.Error.Contains("not found") ? 404 : 500);
            SendIndicatorJsonResponse(context.Response, statusCode, result);
        }

        private void SendJsonResponse(HttpListenerResponse response, int statusCode, QueryResponse body)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json; charset=utf-8";

            var json = JsonConvert.SerializeObject(body);
            var buffer = Encoding.UTF8.GetBytes(json);

            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.Close();
        }

        private void SendIndicatorJsonResponse(HttpListenerResponse response, int statusCode, IndicatorResult body)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json; charset=utf-8";

            var json = JsonConvert.SerializeObject(body);
            var buffer = Encoding.UTF8.GetBytes(json);

            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.Close();
        }

        private void Log(string message)
        {
            if (_logCallback != null)
            {
                _logCallback(message);
            }
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;
            return value.Substring(0, maxLength) + "...";
        }

        public void Dispose()
        {
            Stop();
            if (_cts != null) { _cts.Dispose(); _cts = null; }
            if (_listener != null) { _listener.Close(); ((IDisposable)_listener).Dispose(); _listener = null; }
        }
    }
}
