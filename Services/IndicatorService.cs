using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using DataApiServiceForm.Models;
using Oracle.ManagedDataAccess.Client;

namespace DataApiServiceForm.Services
{
    public class IndicatorService
    {
        private readonly Func<string> _connectionStringProvider;
        private readonly OracleQueryExecutor _executor;

        public IndicatorService(Func<string> connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _executor = new OracleQueryExecutor();
        }

        // ============ Category CRUD ============

        public List<IndicatorCategory> GetCategories()
        {
            var sql = "SELECT CATEGORY_ID, CATEGORY_CODE, CATEGORY_NAME, PARENT_ID, SORT_ORDER, STATUS, CREATE_TIME, UPDATE_TIME FROM INDICATOR_CATEGORY ORDER BY SORT_ORDER, CATEGORY_ID";
            List<string> columns;
            var rows = _executor.ExecuteQuery(_connectionStringProvider(), sql, out columns);
            return MapCategories(rows);
        }

        public void SaveCategory(IndicatorCategory category)
        {
            var cs = _connectionStringProvider();
            using (var conn = new OracleConnection(cs))
            {
                conn.Open();
                if (category.CategoryId == 0)
                {
                    var sql = @"INSERT INTO INDICATOR_CATEGORY (CATEGORY_ID, CATEGORY_CODE, CATEGORY_NAME, PARENT_ID, SORT_ORDER, STATUS, CREATE_TIME)
                                VALUES (SEQ_INDICATOR_CATEGORY.NEXTVAL, :code, :name, :parentId, :sortOrder, :status, SYSDATE)";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("code", category.CategoryCode);
                        cmd.Parameters.Add("name", category.CategoryName);
                        cmd.Parameters.Add("parentId", category.ParentId);
                        cmd.Parameters.Add("sortOrder", category.SortOrder);
                        cmd.Parameters.Add("status", category.Status);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    var sql = @"UPDATE INDICATOR_CATEGORY SET CATEGORY_CODE=:code, CATEGORY_NAME=:name, PARENT_ID=:parentId, SORT_ORDER=:sortOrder, STATUS=:status, UPDATE_TIME=SYSDATE WHERE CATEGORY_ID=:id";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("code", category.CategoryCode);
                        cmd.Parameters.Add("name", category.CategoryName);
                        cmd.Parameters.Add("parentId", category.ParentId);
                        cmd.Parameters.Add("sortOrder", category.SortOrder);
                        cmd.Parameters.Add("status", category.Status);
                        cmd.Parameters.Add("id", category.CategoryId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void DeleteCategory(int categoryId)
        {
            var cs = _connectionStringProvider();
            using (var conn = new OracleConnection(cs))
            {
                conn.Open();
                var sql = "DELETE FROM INDICATOR_CATEGORY WHERE CATEGORY_ID=:id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", categoryId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ============ Indicator CRUD ============

        public List<IndicatorDef> GetAllIndicators()
        {
            var sql = @"SELECT d.INDICATOR_ID, d.INDICATOR_CODE, d.INDICATOR_NAME, d.CATEGORY_ID, d.QUERY_SQL, d.PARAM_DEF,
                               d.DESCRIPTION, d.DATA_TYPE, d.UNIT, d.STATUS, d.VERSION,
                               d.CREATE_TIME, d.UPDATE_TIME, d.CREATE_USER, d.UPDATE_USER,
                               c.CATEGORY_NAME
                        FROM INDICATOR_DEF d
                        LEFT JOIN INDICATOR_CATEGORY c ON d.CATEGORY_ID = c.CATEGORY_ID
                        ORDER BY d.INDICATOR_ID";
            List<string> columns;
            var rows = _executor.ExecuteQuery(_connectionStringProvider(), sql, out columns);
            return MapIndicators(rows);
        }

        public List<IndicatorDef> GetIndicatorsByCategory(int categoryId)
        {
            var cs = _connectionStringProvider();
            var results = new List<IndicatorDef>();
            using (var conn = new OracleConnection(cs))
            {
                conn.Open();
                var sql = @"SELECT d.INDICATOR_ID, d.INDICATOR_CODE, d.INDICATOR_NAME, d.CATEGORY_ID, d.QUERY_SQL, d.PARAM_DEF,
                                   d.DESCRIPTION, d.DATA_TYPE, d.UNIT, d.STATUS, d.VERSION,
                                   d.CREATE_TIME, d.UPDATE_TIME, d.CREATE_USER, d.UPDATE_USER,
                                   c.CATEGORY_NAME
                            FROM INDICATOR_DEF d
                            LEFT JOIN INDICATOR_CATEGORY c ON d.CATEGORY_ID = c.CATEGORY_ID
                            WHERE d.CATEGORY_ID = :catId
                            ORDER BY d.INDICATOR_ID";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("catId", categoryId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(MapIndicator(reader));
                        }
                    }
                }
            }
            return results;
        }

        public IndicatorDef GetIndicatorByCode(string code)
        {
            var cs = _connectionStringProvider();
            using (var conn = new OracleConnection(cs))
            {
                conn.Open();
                var sql = @"SELECT d.INDICATOR_ID, d.INDICATOR_CODE, d.INDICATOR_NAME, d.CATEGORY_ID, d.QUERY_SQL, d.PARAM_DEF,
                                   d.DESCRIPTION, d.DATA_TYPE, d.UNIT, d.STATUS, d.VERSION,
                                   d.CREATE_TIME, d.UPDATE_TIME, d.CREATE_USER, d.UPDATE_USER,
                                   c.CATEGORY_NAME
                            FROM INDICATOR_DEF d
                            LEFT JOIN INDICATOR_CATEGORY c ON d.CATEGORY_ID = c.CATEGORY_ID
                            WHERE d.INDICATOR_CODE = :code";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("code", code);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapIndicator(reader);
                        }
                    }
                }
            }
            return null;
        }

        public void SaveIndicator(IndicatorDef indicator)
        {
            var cs = _connectionStringProvider();
            using (var conn = new OracleConnection(cs))
            {
                conn.Open();
                if (indicator.IndicatorId == 0)
                {
                    var sql = @"INSERT INTO INDICATOR_DEF (INDICATOR_ID, INDICATOR_CODE, INDICATOR_NAME, CATEGORY_ID, QUERY_SQL, PARAM_DEF, DESCRIPTION, DATA_TYPE, UNIT, STATUS, VERSION, CREATE_TIME, CREATE_USER)
                                VALUES (SEQ_INDICATOR_DEF.NEXTVAL, :code, :name, :catId, :querySql, :paramDef, :desc, :dataType, :unit, :status, 1, SYSDATE, :createUser)";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("code", indicator.IndicatorCode);
                        cmd.Parameters.Add("name", indicator.IndicatorName);
                        cmd.Parameters.Add("catId", (object)indicator.CategoryId ?? DBNull.Value);
                        cmd.Parameters.Add("querySql", (object)indicator.QuerySql ?? DBNull.Value);
                        cmd.Parameters.Add("paramDef", (object)indicator.ParamDef ?? DBNull.Value);
                        cmd.Parameters.Add("desc", (object)indicator.Description ?? DBNull.Value);
                        cmd.Parameters.Add("dataType", indicator.DataType);
                        cmd.Parameters.Add("unit", (object)indicator.Unit ?? DBNull.Value);
                        cmd.Parameters.Add("status", indicator.Status);
                        cmd.Parameters.Add("createUser", (object)indicator.CreateUser ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    var sql = @"UPDATE INDICATOR_DEF SET INDICATOR_CODE=:code, INDICATOR_NAME=:name, CATEGORY_ID=:catId, QUERY_SQL=:querySql, PARAM_DEF=:paramDef, DESCRIPTION=:desc, DATA_TYPE=:dataType, UNIT=:unit, STATUS=:status, VERSION=VERSION+1, UPDATE_TIME=SYSDATE, UPDATE_USER=:updateUser WHERE INDICATOR_ID=:id";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("code", indicator.IndicatorCode);
                        cmd.Parameters.Add("name", indicator.IndicatorName);
                        cmd.Parameters.Add("catId", (object)indicator.CategoryId ?? DBNull.Value);
                        cmd.Parameters.Add("querySql", (object)indicator.QuerySql ?? DBNull.Value);
                        cmd.Parameters.Add("paramDef", (object)indicator.ParamDef ?? DBNull.Value);
                        cmd.Parameters.Add("desc", (object)indicator.Description ?? DBNull.Value);
                        cmd.Parameters.Add("dataType", indicator.DataType);
                        cmd.Parameters.Add("unit", (object)indicator.Unit ?? DBNull.Value);
                        cmd.Parameters.Add("status", indicator.Status);
                        cmd.Parameters.Add("updateUser", (object)indicator.UpdateUser ?? DBNull.Value);
                        cmd.Parameters.Add("id", indicator.IndicatorId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void DeleteIndicator(int indicatorId)
        {
            var cs = _connectionStringProvider();
            using (var conn = new OracleConnection(cs))
            {
                conn.Open();
                var sql = "DELETE FROM INDICATOR_DEF WHERE INDICATOR_ID=:id";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("id", indicatorId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ============ Indicator Execution ============

        public IndicatorResult ExecuteIndicator(string code, Dictionary<string, object> parameters)
        {
            var indicator = GetIndicatorByCode(code);
            if (indicator == null)
            {
                return IndicatorResult.Fail(string.Format("Indicator not found: {0}", code));
            }

            if (indicator.Status != "1")
            {
                return IndicatorResult.Fail(string.Format("Indicator is disabled: {0}", code));
            }

            if (string.IsNullOrWhiteSpace(indicator.QuerySql))
            {
                return IndicatorResult.Fail(string.Format("Indicator has no QuerySql: {0}", code));
            }

            // Replace #{param} placeholders with actual values
            var finalSql = indicator.QuerySql;
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    var placeholder = "#{" + kvp.Key + "}";
                    var value = kvp.Value != null ? kvp.Value.ToString() : "";
                    finalSql = finalSql.Replace(placeholder, value);
                }
            }

            // Execute
            try
            {
                List<string> columns;
                var data = _executor.ExecuteQuery(_connectionStringProvider(), finalSql, out columns);

                var result = IndicatorResult.Ok(data);
                result.IndicatorCode = indicator.IndicatorCode;
                result.IndicatorName = indicator.IndicatorName;
                result.DataType = indicator.DataType;
                result.Unit = indicator.Unit;
                result.Columns = columns;
                result.RowCount = data.Count;
                result.ExecuteTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                return result;
            }
            catch (OracleException ex)
            {
                return IndicatorResult.Fail(string.Format("Oracle error: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                return IndicatorResult.Fail(string.Format("Execution error: {0}", ex.Message));
            }
        }

        // ============ Mapping Helpers ============

        private List<IndicatorCategory> MapCategories(List<Dictionary<string, object>> rows)
        {
            var list = new List<IndicatorCategory>();
            foreach (var row in rows)
            {
                list.Add(new IndicatorCategory
                {
                    CategoryId = Convert.ToInt32(row["CATEGORY_ID"]),
                    CategoryCode = row["CATEGORY_CODE"]?.ToString(),
                    CategoryName = row["CATEGORY_NAME"]?.ToString(),
                    ParentId = Convert.ToInt32(row["PARENT_ID"]),
                    SortOrder = Convert.ToInt32(row["SORT_ORDER"]),
                    Status = row["STATUS"]?.ToString()?.Trim(),
                    CreateTime = row["CREATE_TIME"] as DateTime?,
                    UpdateTime = row["UPDATE_TIME"] as DateTime?
                });
            }
            return list;
        }

        private List<IndicatorDef> MapIndicators(List<Dictionary<string, object>> rows)
        {
            var list = new List<IndicatorDef>();
            foreach (var row in rows)
            {
                list.Add(MapIndicatorRow(row));
            }
            return list;
        }

        private IndicatorDef MapIndicator(OracleDataReader reader)
        {
            return new IndicatorDef
            {
                IndicatorId = Convert.ToInt32(reader["INDICATOR_ID"]),
                IndicatorCode = reader["INDICATOR_CODE"]?.ToString(),
                IndicatorName = reader["INDICATOR_NAME"]?.ToString(),
                CategoryId = reader["CATEGORY_ID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["CATEGORY_ID"]),
                QuerySql = reader["QUERY_SQL"]?.ToString(),
                ParamDef = reader["PARAM_DEF"]?.ToString(),
                Description = reader["DESCRIPTION"]?.ToString(),
                DataType = reader["DATA_TYPE"]?.ToString(),
                Unit = reader["UNIT"]?.ToString(),
                Status = reader["STATUS"]?.ToString()?.Trim(),
                Version = Convert.ToInt32(reader["VERSION"]),
                CreateTime = reader["CREATE_TIME"] as DateTime?,
                UpdateTime = reader["UPDATE_TIME"] as DateTime?,
                CreateUser = reader["CREATE_USER"]?.ToString(),
                UpdateUser = reader["UPDATE_USER"]?.ToString(),
                CategoryName = reader["CATEGORY_NAME"]?.ToString()
            };
        }

        private IndicatorDef MapIndicatorRow(Dictionary<string, object> row)
        {
            return new IndicatorDef
            {
                IndicatorId = Convert.ToInt32(row["INDICATOR_ID"]),
                IndicatorCode = row["INDICATOR_CODE"]?.ToString(),
                IndicatorName = row["INDICATOR_NAME"]?.ToString(),
                CategoryId = row["CATEGORY_ID"] == null ? (int?)null : Convert.ToInt32(row["CATEGORY_ID"]),
                QuerySql = row["QUERY_SQL"]?.ToString(),
                ParamDef = row["PARAM_DEF"]?.ToString(),
                Description = row["DESCRIPTION"]?.ToString(),
                DataType = row["DATA_TYPE"]?.ToString(),
                Unit = row["UNIT"]?.ToString(),
                Status = row["STATUS"]?.ToString()?.Trim(),
                Version = Convert.ToInt32(row["VERSION"]),
                CreateTime = row["CREATE_TIME"] as DateTime?,
                UpdateTime = row["UPDATE_TIME"] as DateTime?,
                CreateUser = row["CREATE_USER"]?.ToString(),
                UpdateUser = row["UPDATE_USER"]?.ToString(),
                CategoryName = row.ContainsKey("CATEGORY_NAME") ? row["CATEGORY_NAME"]?.ToString() : null
            };
        }
    }
}
