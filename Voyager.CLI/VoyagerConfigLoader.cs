using System.Text.Json;
using Serilog;
using Serilog.Context;
using Voyager.Enricher;

namespace Voyager.CLI;

public class VoyagerConfigLoader
{
    public static ITransportConfigure TransportLoad(string path)
    {
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.With(new VoyagerEnricherLogBase(typeof(VoyagerConfigLoader)))
            .WriteTo.Console(outputTemplate: VoyagerEnricherLogBase.OutputTemplate)
            .CreateLogger();
        LogContext.PushProperty("Method", nameof(TransportLoad));
        
        logger.Information("Load transport configuration");
        
         string jsonString = File.ReadAllText(path);
         ITransportConfigure? configure;
         if (jsonString.Contains("privateKey", StringComparison.OrdinalIgnoreCase))
         {
             configure = JsonSerializer.Deserialize<TransportConfigure>(jsonString);
             logger.Information("Load {TransportConfigureType}", nameof(TransportConfigure));
         }
         else
         {
             configure = JsonSerializer.Deserialize<MinimumTransportConfigure>(jsonString);
             logger.Information("Load {TransportConfigureType}", nameof(MinimumTransportConfigure));
         }

         return configure ?? throw new ArgumentException();
    }
    
    public static PeerInfosConfig PeerInfosLoad(string path)
    {
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.With(new VoyagerEnricherLogBase(typeof(VoyagerConfigLoader)))
            .WriteTo.Console(outputTemplate: VoyagerEnricherLogBase.OutputTemplate)
            .CreateLogger();
        LogContext.PushProperty("Method", nameof(PeerInfosLoad));
        
        logger.Information("Load peer infos configuration");

        string jsonString = File.ReadAllText(path);
        return JsonSerializer.Deserialize<PeerInfosConfig>(jsonString) ?? throw new ArgumentException();

    }
}