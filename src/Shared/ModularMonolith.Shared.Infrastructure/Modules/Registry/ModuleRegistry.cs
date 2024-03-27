using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Modules.Registry;

internal sealed class ModuleRegistry : IModuleRegistry
{
    private readonly List<MessageEndpointRegistration> _endpointRegistrations = new();

    public IEnumerable<MessageEndpointRegistration> GetEndpointRegistrations(string key)
        => _endpointRegistrations.Where(x => x.Key == key);

    public void AddEndpointRegistration(Type receiverType, Func<IMessage, Task> action)
    {
        if (string.IsNullOrWhiteSpace(receiverType.Namespace))
        {
            throw new InvalidOperationException("Cannot register dynamic type.");
        }
        
        var registration = new MessageEndpointRegistration(receiverType, action);
        _endpointRegistrations.Add(registration);
    }
}