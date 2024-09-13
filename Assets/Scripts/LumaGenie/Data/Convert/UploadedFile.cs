using Newtonsoft.Json;

namespace LumaGenie.Data.Convert
{
    public class UploadedFile
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }
        [JsonProperty("file_url")]
        public string FileUrl { get; set; }
        [JsonProperty("metadata")]
        public FileMetadata Metadata { get; set; } = new();
    }
}