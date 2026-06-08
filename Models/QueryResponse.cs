using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataApiServiceForm.Models
{
    public class QueryResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<Dictionary<string, object>> Data { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; }

        [JsonProperty("rowCount")]
        public int RowCount { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public static QueryResponse Ok(List<Dictionary<string, object>> data, List<string> columns)
        {
            return new QueryResponse
            {
                Success = true,
                Data = data,
                Columns = columns,
                RowCount = data != null ? data.Count : 0,
                Error = null
            };
        }

        public static QueryResponse Fail(string error)
        {
            return new QueryResponse
            {
                Success = false,
                Data = null,
                Columns = null,
                RowCount = 0,
                Error = error
            };
        }
    }
}
