using System.Reflection;
using Caracal.Messaging.Routing.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing;

public sealed class RouteingFactory : IRouteingFactory
{
    private readonly IOptions<RoutingOptions> _options;
    private readonly List<ITerminal> _terminals = new();
    private readonly List<IProcessor> _processors = new();

    public RouteingFactory(IOptions<RoutingOptions> options)
    {
        _options = options;

        InitializeTerminals();
        InitializeProcessors();
    }
    
    private void InitializeTerminals()
    {
        foreach (var terminalOptions in _options.Value.Terminals)
        {
            var type = Assembly.Load(terminalOptions.Assembly).GetType(terminalOptions.Type);
            if (type == null) continue;
            
            var instance = Activator.CreateInstance(type, terminalOptions);
            if(instance is not ITerminal terminal) continue;
            
            _terminals.Add(terminal);
        }
    }
    
    private void InitializeProcessors()
    {
        foreach (var processorOptions in _options.Value.Processors)
        {
            var type = Assembly.Load(processorOptions.Assembly).GetType(processorOptions.Type);
            if (type == null) continue;
            
            var instance = Activator.CreateInstance(type, processorOptions);
            if(instance is not IProcessor processor) continue;

            processor.Terminals = _terminals.Where(t => processorOptions.Terminals.Contains(t.Id)).ToArray();
            _processors.Add(processor);
        }
    }

    public IEnumerable<IProcessor> GetProcessors() => _processors;
}