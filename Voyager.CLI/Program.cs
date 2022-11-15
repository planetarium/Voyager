// See https://aka.ms/new-console-template for more information

using Cocona;
using Voyager;
using Voyager.CLI;

CoconaApp.Run(async (string configPath, string peerInfosConfigPath) =>
{
    ITransportConfigure transportConfigure = VoyagerConfigLoader.TransportLoad(configPath);
    PeerInfosConfig peerInfosConfig = VoyagerConfigLoader.PeerInfosLoad(peerInfosConfigPath);

    var voyager = new Voyager.Voyager(transportConfigure);
    foreach (var peer in peerInfosConfig.BoundPeers)
    {
        bool result = await voyager.Ping(peer);
        Console.WriteLine($"Peer {peer.EndPoint.ToString()} return {result} about Ping.");
    }
});
