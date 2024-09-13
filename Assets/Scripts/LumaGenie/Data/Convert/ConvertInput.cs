using Newtonsoft.Json;

namespace LumaGenie.Data.Convert
{
    public class ConvertInput
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = "blender_convert";
        [JsonProperty("convert")]
        public ConvertJobParams Convert { get; set; } = new();
    }
}