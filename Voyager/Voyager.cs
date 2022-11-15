using System.Collections.Immutable;
using Libplanet.Crypto;
using Libplanet.Net;
using Libplanet.Net.Messages;
using Libplanet.Net.Transports;
using Nito.AsyncEx;
using Serilog;
using Voyager.Enricher;
using ILogger = Serilog.ILogger;

namespace Voyager;

public class Voyager
{
    private readonly ITransport _transport;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CancellationToken _ctx;
    private readonly ILogger _logger;

    public Voyager(ITransportConfigure configure)
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.With(new VoyagerEnricherLogBase(this))
            .WriteTo.Console(outputTemplate: VoyagerEnricherLogBase.OutputTemplate)
            .CreateLogger();
        
        _logger.Debug("Start to initialize {Class}", nameof(Voyager));

        _transport = configure switch
        {
            MinimumTransportConfigure minConf => AsyncContext.Run(async () => await NetMQTransport.Create(
                new PrivateKey(), minConf.AppProtocolVersion,
                minConf.TrustedAppProtocolVersionSigners.ToImmutableHashSet(), 50, null, null, minConf.IceServers, null,
                null)),
            TransportConfigure conf => AsyncContext.Run(async () => await NetMQTransport.Create(conf.PrivateKey,
                conf.AppProtocolVersion, conf.TrustedAppProtocolVersionSigners.ToImmutableHashSet(), 50, null, null,
                conf.IceServers, null, null)),
            _ => throw new ArgumentException()
        };
        _cancellationTokenSource = new CancellationTokenSource();
        _ctx = _cancellationTokenSource.Token;
    }

    public async Task<Message?> SendMessage(Message message, BoundPeer target, CancellationToken token)
    {
        _logger.Debug("Start to {Method}", nameof(SendMessage));
        using CancellationTokenSource linkedCts =
            CancellationTokenSource.CreateLinkedTokenSource(_ctx, token);
        try
        {
            try
            {
                Message received =
                    await _transport.SendMessageAsync(target, message, TimeSpan.FromSeconds(1), linkedCts.Token);
                return received;
            }
            catch (CommunicationFailException failException)
            {
                _logger.Debug(
                    "Message to {BoundPeer} is failed. Message: {Message} Inner: {InnerException}",
                    failException.Peer,
                    failException.MessageType,
                    failException.InnerException?.GetType());
            }
            catch (ObjectDisposedException disposedException)
            {
                _logger.Error("{Class} connection is already disposed", nameof(Voyager));
                throw;
            }
        }
        catch (OperationCanceledException) {
            if (_ctx.IsCancellationRequested) {
                _ctx.ThrowIfCancellationRequested();
            }
            else if (token.IsCancellationRequested) {
                token.ThrowIfCancellationRequested();
            }
        }

        return null;
    }

    public async Task<bool> Ping(BoundPeer target) => await SendMessage(new PingMsg(), target, _ctx) is not null;
}