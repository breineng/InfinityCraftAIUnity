using System;
using LumaGenie.Data.Convert;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LumaGenie.Converters
{
    public class ExportFormatConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ConvertJobParams jobParams = (ConvertJobParams)value;
            writer.WriteStartObject();
            writer.WritePropertyName($"export_{jobParams.Format.ToString().ToLower()}");
            writer.WriteValue(true);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Name.StartsWith("export_") && property.Value.Type == JTokenType.Boolean &&
                    (bool)property.Value)
                {
                    var formatString = property.Name.Substring("export_".Length);
                    if (Enum.TryParse(formatString, true, out ExportFormat format))
                    {
                        return new ConvertJobParams() { Format = format };
                    }
                }
            }

            throw new JsonSerializationException("Invalid export format");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ConvertJobParams);
        }
    }
}