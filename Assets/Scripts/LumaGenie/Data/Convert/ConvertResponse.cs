using Newtonsoft.Json;

namespace LumaGenie.Data.Convert
{
    public class ConvertResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("response")]
        public ConvertResponseData Response { get; set; } = new();
    }
}