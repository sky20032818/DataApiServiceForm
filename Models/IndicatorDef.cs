using System;
using Newtonsoft.Json;

namespace DataApiServiceForm.Models
{
    public class IndicatorDef
    {
        [JsonProperty("indicatorId")]
        public int IndicatorId { get; set; }

        [JsonProperty("indicatorCode")]
        public string IndicatorCode { get; set; }

        [JsonProperty("indicatorName")]
        public string IndicatorName { get; set; }

        [JsonProperty("categoryId")]
        public int? CategoryId { get; set; }

        [JsonProperty("querySql")]
        public string QuerySql { get; set; }

        [JsonProperty("paramDef")]
        public string ParamDef { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        public IndicatorDef()
        {
            DataType = "LIST";
            Status = "1";
            Version = 1;
        }

        [JsonProperty("createTime")]
        public DateTime? CreateTime { get; set; }

        [JsonProperty("updateTime")]
        public DateTime? UpdateTime { get; set; }

        [JsonProperty("createUser")]
        public string CreateUser { get; set; }

        [JsonProperty("updateUser")]
        public string UpdateUser { get; set; }

        // UI display helpers (not persisted)
        [JsonIgnore]
        public string CategoryName { get; set; }

        [JsonIgnore]
        public string StatusText
        {
            get { return Status == "1" ? "启用" : "禁用"; }
        }

        [JsonIgnore]
        public string DataTypeText
        {
            get { return DataType == "SINGLE" ? "单值" : "列表"; }
        }
    }
}
