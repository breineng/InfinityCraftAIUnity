using System;
using Newtonsoft.Json;

namespace LumaGenie.Data.Convert
{
    public class ConvertResponseData
    {
        [JsonProperty("uploaded_files")]
        public UploadedFile[] UploadedFiles { get; set; } = Array.Empty<UploadedFile>();
    }
}