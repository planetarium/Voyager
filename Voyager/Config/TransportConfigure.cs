using System.Text.Json.Serialization;
using Libplanet.Crypto;
using Libplanet.Net;

namespace Voyager;

public class TransportConfigure : ITransportConfigure
{
    [JsonConverter(typeof(AppProtocolVersionConverter))]
    public AppProtocolVersion AppProtocolVersion { get; set; }

    [JsonConverter(typeof(TrustedAPVSignersConverter))]
    public List<PublicKey> TrustedAppProtocolVersionSigners { get; set; }

    [JsonConverter(typeof(IceServersConverter))]
    public List<IceServer> IceServers { get; set; }

    [JsonConverter(typeof(PrivateKeyConverter))]
    public PrivateKey PrivateKey { get; set; }
}