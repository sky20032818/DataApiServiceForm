using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DataApiServiceForm.Services
{
    public class OracleQueryExecutor
    {
        public List<Dictionary<string, object>> ExecuteQuery(string connectionString, string sql, out List<string> columns)
        {
            var results = new List<Dictionary<string, object>>();
            columns = new List<string>();

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        // Read column names
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columns.Add(reader.GetName(i));
                        }

                        // Read each row
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var value = reader.GetValue(i);
                                row[columns[i]] = value == DBNull.Value ? null : value;
                            }
                            results.Add(row);
                        }
                    }
                }
            }

            return results;
        }
    }
}
