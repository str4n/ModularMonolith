using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Modules.Registry;

internal sealed class ModuleRegistry : IModuleRegistry
{
    private readonly List<MessageRegistration> _messageRegistrations = new();

    public IEnumerable<MessageRegistration> GetMessageRegistrations(string key)
        => _messageRegistrations.Where(x => x.Key == key);

    public void AddMessageRegistration(Type receiverType, Func<IMessage, Task> action)
    {
        if (string.IsNullOrWhiteSpace(receiverType.Namespace))
        {
            throw new InvalidOperationException("Cannot register dynamic type.");
        }
        
        var registration = new MessageRegistration(receiverType, action);
        _messageRegistrations.Add(registration);
    }
}