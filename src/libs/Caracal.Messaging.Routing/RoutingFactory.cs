using Caracal.Lang;
using Caracal.Messaging.Routing.Config;
using Microsoft.Extensions.Options;

namespace Caracal.Messaging.Routing;

public sealed class RoutingFactory : IRoutingFactory
{
    private readonly IOptions<RoutingOptions> _options;
    private readonly List<ITerminal> _terminals = new();
    private readonly List<IProcessor> _processors = new();

    public RoutingFactory(IOptions<RoutingOptions> options)
    {
        _options = options;

        InitializeTerminals();
        InitializeProcessors();
    }
    
    public IEnumerable<IProcessor> GetProcessors() => _processors;
    
    private void InitializeTerminals()
    {
        foreach (var terminalOptions in _options.Value.Terminals)
        {
            var terminal = terminalOptions.CreateObjectFromOption<ITerminal>();
            if (terminal == null) continue;
            
            _terminals.Add(terminal);
        }
    }
    
    private void InitializeProcessors()
    {
        foreach (var processorOptions in _options.Value.Processors)
        {
            var processor = processorOptions.CreateObjectFromOption<IProcessor>();
            if (processor == null) continue;
            
            processor.Terminals = _terminals.Where(t => processorOptions.Terminals.Contains(t.Id)).ToArray();
            _processors.Add(processor);
        }
    }
}