using ModularMonolith.Shared.Abstractions.Messaging;
using ModularMonolith.Shared.Abstractions.Modules;
using ModularMonolith.Shared.Infrastructure.Modules.Registry;
using ModularMonolith.Shared.Infrastructure.Modules.Serialization;

namespace ModularMonolith.Shared.Infrastructure.Modules.Client;

internal sealed class ModuleClient : IModuleClient
{
    private readonly IModuleRegistry _moduleRegistry;
    private readonly IModuleSerializer _moduleSerializer;

    public ModuleClient(IModuleRegistry moduleRegistry, IModuleSerializer moduleSerializer)
    {
        _moduleRegistry = moduleRegistry;
        _moduleSerializer = moduleSerializer;
    }
    
    public async Task PublishAsync(IMessage message)
    {
        var key = message.GetType().Name;
        var registrations = _moduleRegistry.GetEndpointRegistrations(key);

        var tasks = new List<Task>();
        
        foreach (var registration in registrations)
        {
            var handle = registration.HandleAsync;
            var receiverMessage = TranslateType(message, registration.ReceiverType);
            
            tasks.Add(handle(receiverMessage));
        }

        await Task.WhenAll(tasks);
    }

    private IMessage TranslateType(IMessage message, Type receiverType)
        => _moduleSerializer.Deserialize(_moduleSerializer.Serialize(message), receiverType) as IMessage;
}