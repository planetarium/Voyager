# Voyager

Voyager is a tool for Libplanet.Net.

# Usage
```
dotnet run --project .\Voyager.CLI -- --help
Usage: Voyager.CLI [--config-path <String>] [--peer-infos-config-path <String>] [--help] [--version]

Voyager.CLI

Options:
  --config-path <String>                (Required)
  --peer-infos-config-path <String>     (Required)
  -h, --help                           Show help message
  --version                            Show version
```

# Config
## TransportConfig
```json
{
  "AppProtocolVersion": "<APV>",
  "TrustedAppProtocolVersionSigners": "<PublicKey>",
  "IceServers": "<IceServer>"
}
```

## PeerInfosConfig
```json
{
  "BoundPeers": "<BoundPeer>"
}
```
