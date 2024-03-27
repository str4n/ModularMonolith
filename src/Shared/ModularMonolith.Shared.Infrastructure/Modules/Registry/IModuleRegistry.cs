using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Modules.Registry;

internal interface IModuleRegistry
{
    IEnumerable<MessageRegistration> GetMessageRegistrations(string key);
    void AddMessageRegistration(Type receiverType, Func<IMessage, Task> action);
}