using System;
using Newtonsoft.Json;

namespace DataApiServiceForm.Models
{
    public class IndicatorCategory
    {
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("categoryCode")]
        public string CategoryCode { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createTime")]
        public DateTime? CreateTime { get; set; }

        [JsonProperty("updateTime")]
        public DateTime? UpdateTime { get; set; }

        public IndicatorCategory()
        {
            Status = "1";
        }
    }
}
