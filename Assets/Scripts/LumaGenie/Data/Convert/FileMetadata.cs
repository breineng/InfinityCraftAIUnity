using Newtonsoft.Json;

namespace LumaGenie.Data.Convert
{
    public class FileMetadata
    {
        [JsonProperty("file_size")]
        public string FileSize { get; set; }
        [JsonProperty("uploaded_time")]
        public string UploadedTime { get; set; }
        [JsonProperty("file_extension")]
        public string FileExtension { get; set; }
        [JsonProperty("original_file_name")]
        public string OriginalFileName { get; set; }
    }
}