using System.Text.Json.Serialization;
using Libplanet.Net;

namespace Voyager;

public class PeerInfosConfig
{
    [JsonConverter(typeof(BoundPeerConverter))]
    public List<BoundPeer> BoundPeers { get; set; }
}