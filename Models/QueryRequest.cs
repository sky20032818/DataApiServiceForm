using Newtonsoft.Json;

namespace DataApiServiceForm.Models
{
    public class QueryRequest
    {
        [JsonProperty("sql")]
        public string Sql { get; set; }
    }
}
