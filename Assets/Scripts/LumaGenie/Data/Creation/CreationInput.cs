using Newtonsoft.Json;

namespace LumaGenie.Data.Creation
{
    public class CreationInput
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("jobParams")]
        public CreationJobParams JobParams { get; set; } = new();
    }
}