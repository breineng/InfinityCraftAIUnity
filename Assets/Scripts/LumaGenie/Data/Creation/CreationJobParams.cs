using System;
using Newtonsoft.Json;

namespace LumaGenie.Data.Creation
{
    public class CreationJobParams
    {
        [JsonProperty("seed")]
        public string Seed { get; set; }
        [JsonProperty("previousSeeds")]
        public string[] PreviousSeeds { get; set; } = Array.Empty<string>();
    }
}