using LumaGenie.Data.Creation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LumaGenie.Data.Status
{
    public class StatusResponseData
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("status")][JsonConverter(typeof(StringEnumConverter))] 
        public CreationStatus Status { get; set; }
        [JsonProperty("input")]
        public CreationInput Input { get; set; } = new();
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("metadata")]
        public StatusMetadata Metadata { get; set; } = new();
    }
}