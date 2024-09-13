using Newtonsoft.Json;

namespace LumaGenie.Data.Creation
{
    public class CreationResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("response")]
        public string[] Response { get; set; }
    }
}