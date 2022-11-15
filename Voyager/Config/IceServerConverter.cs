using System.Text.Json;
using System.Text.Json.Serialization;
using Libplanet.Net;

namespace Voyager;

public class IceServersConverter : JsonConverter<List<IceServer>>
{
    public override List<IceServer> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()?.Split(',').Select(x => new IceServer(x)).ToList();

    public override void Write(Utf8JsonWriter writer, List<IceServer> value, JsonSerializerOptions options)
        => writer.WriteStringValue(string.Join(",", value.Select(x => x.Url)));
}