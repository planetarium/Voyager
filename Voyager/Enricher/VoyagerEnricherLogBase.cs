using Serilog.Core;
using Serilog.Events;

namespace Voyager.Enricher;

public class VoyagerEnricherLogBase : ILogEventEnricher
{
    private readonly string _className;
    
    public VoyagerEnricherLogBase(Object e)
    {
        _className = e.GetType().Name;
    }

    public virtual void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(
            propertyFactory.CreateProperty("ThreadId", Thread.CurrentThread.ManagedThreadId, false));
        logEvent.AddPropertyIfAbsent(
            propertyFactory.CreateProperty("Source", _className, false));
    }
    
    public static string OutputTemplate => "[{Timestamp:HH:mm:ss} {Level:u3}] [{ThreadId}] [{Source}] {Message:lj}{NewLine}{Exception}";
    
}