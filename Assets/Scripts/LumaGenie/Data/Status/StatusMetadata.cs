using System;
using Newtonsoft.Json;

namespace LumaGenie.Data.Status
{
    public class StatusMetadata
    {
        [JsonProperty("progress")]
        public StatusProgress Progress { get; set; } = new();
        [JsonProperty("interactionUuid")]
        public string InteractionUuid { get; set; }
        [JsonProperty("linkedCreations")]
        public string[] LinkedCreations { get; set; } = Array.Empty<string>();
    }
}