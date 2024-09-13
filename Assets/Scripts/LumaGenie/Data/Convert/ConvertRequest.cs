using System;
using Newtonsoft.Json;

namespace LumaGenie.Data.Convert
{
    public class ConvertRequest
    {
        [JsonProperty("input")]
        public ConvertInput Input { get; set; }
        [JsonProperty("linkedCreations")]
        public string[] LinkedCreations { get; set; } = Array.Empty<string>();

        public ConvertRequest(string uuid, ExportFormat format)
        {
            Input = new ConvertInput()
            {
                Uuid = uuid,
                Type = "blender_convert",
                Convert = new ConvertJobParams()
                {
                    Format = format
                }
            };
        }
    }
}