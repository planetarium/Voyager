using System.Text.Json;
using System.Text.Json.Serialization;
using Libplanet.Net;

namespace Voyager;

public class BoundPeerConverter : JsonConverter<List<BoundPeer>>
{
    public override List<BoundPeer> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new List<BoundPeer>()
        {
            BoundPeer.ParsePeer(reader.GetString() ?? throw new InvalidOperationException())
        };

    public override void Write(Utf8JsonWriter writer, List<BoundPeer> value, JsonSerializerOptions options)
        => writer.WriteStringValue(string.Join(",", value.Select(x => x.PeerString)));
}