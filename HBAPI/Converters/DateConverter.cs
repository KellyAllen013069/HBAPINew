using System.Text.Json;
using System.Text.Json.Serialization;

namespace HBAPI.Converters
{
    public class DateOnlyConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();

            if (string.IsNullOrEmpty(dateString))
            {
                throw new JsonException("Invalid date format or null value encountered.");
            }

            return DateTime.Parse(dateString);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}