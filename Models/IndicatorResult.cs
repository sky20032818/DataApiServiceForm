using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataApiServiceForm.Models
{
    public class IndicatorResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        // Indicator-specific fields
        [JsonProperty("indicatorCode", NullValueHandling = NullValueHandling.Ignore)]
        public string IndicatorCode { get; set; }

        [JsonProperty("indicatorName", NullValueHandling = NullValueHandling.Ignore)]
        public string IndicatorName { get; set; }

        [JsonProperty("dataType", NullValueHandling = NullValueHandling.Ignore)]
        public string DataType { get; set; }

        [JsonProperty("unit", NullValueHandling = NullValueHandling.Ignore)]
        public string Unit { get; set; }

        [JsonProperty("columns", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Columns { get; set; }

        [JsonProperty("executeTime", NullValueHandling = NullValueHandling.Ignore)]
        public string ExecuteTime { get; set; }

        [JsonProperty("rowCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? RowCount { get; set; }

        public static IndicatorResult Ok(object data)
        {
            return new IndicatorResult
            {
                Success = true,
                Data = data,
                Error = null
            };
        }

        public static IndicatorResult Fail(string error)
        {
            return new IndicatorResult
            {
                Success = false,
                Data = null,
                Error = error
            };
        }
    }
}
