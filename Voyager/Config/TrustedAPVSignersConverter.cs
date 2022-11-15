using System.Text.Json;
using System.Text.Json.Serialization;
using Libplanet;
using Libplanet.Crypto;

namespace Voyager;

public class TrustedAPVSignersConverter : JsonConverter<List<PublicKey>>
{
    public override List<PublicKey> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()?.Split(',').Select(x => new PublicKey(ByteUtil.ParseHex(x))).ToList();

    public override void Write(Utf8JsonWriter writer, List<PublicKey> value, JsonSerializerOptions options)
        => writer.WriteStringValue(string.Join(",", value.Select(x => x.ToString())));
}