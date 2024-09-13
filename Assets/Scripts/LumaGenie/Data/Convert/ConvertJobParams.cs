using LumaGenie.Converters;
using Newtonsoft.Json;

namespace LumaGenie.Data.Convert
{
    [JsonConverter(typeof(ExportFormatConverter))]
    public class ConvertJobParams
    {
        public ExportFormat Format { get; set; }
    }
}