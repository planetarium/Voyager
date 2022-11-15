using System.Text.Json;
using System.Text.Json.Serialization;
using Libplanet.Net;

namespace Voyager;

public class AppProtocolVersionConverter : JsonConverter<AppProtocolVersion>
{
    public override AppProtocolVersion Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
        => AppProtocolVersion.FromToken(reader.GetString());

    public override void Write(Utf8JsonWriter writer, AppProtocolVersion value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Token);
}
