using System.Text.Json;
using System.Text.Json.Serialization;
using Libplanet;
using Libplanet.Crypto;
using Libplanet.Net;

namespace Voyager;

public class PrivateKeyConverter : JsonConverter<PrivateKey>
{
    public override PrivateKey Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        string value = reader.GetString() ?? throw new InvalidOperationException();
        return PrivateKey.FromString(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, PrivateKey value, JsonSerializerOptions options)
        => writer.WriteStringValue(ByteUtil.Hex(value.ToByteArray()));
}
