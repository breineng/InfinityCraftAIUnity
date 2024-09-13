using System;
using Newtonsoft.Json;

namespace LumaGenie.Data.Creation
{
    public class CreationRequest
    {
        [JsonProperty("input")]
        public CreationInput Input { get; set; }
        [JsonProperty("linkedCreations")]
        public string[] LinkedCreations { get; set; } = Array.Empty<string>();
        [JsonProperty("client")]
        public string Client { get; set; }

        public CreationRequest(string prompt, string seed = null)
        {
            Client = "web";
            Input = new CreationInput()
            {
                Text = prompt,
                Type = "imagine_3d_one",
                JobParams = new CreationJobParams()
                {
                    Seed = seed
                }
            };
        }
    }
}