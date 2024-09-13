using Newtonsoft.Json;

namespace LumaGenie.Data.Status
{
    public class StatusResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("response")]
        public StatusResponseData Response { get; set; } = new();
    }
}