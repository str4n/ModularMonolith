using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Modules.Registry;

internal interface IModuleRegistry
{
    IEnumerable<MessageEndpointRegistration> GetEndpointRegistrations(string key);
    void AddEndpointRegistration(Type receiverType, Func<IMessage, Task> action);
}