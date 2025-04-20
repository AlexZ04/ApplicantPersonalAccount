using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.Infrastructure.Converters
{
    public class JsonTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.SpecifyKind(reader.GetDateTime(), DateTimeKind.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime());
        }
    }
}
