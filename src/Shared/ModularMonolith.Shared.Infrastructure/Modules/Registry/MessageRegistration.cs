using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Modules.Registry;

internal sealed record MessageRegistration(Type ReceiverType, Func<IMessage, Task> HandleAsync)
{
    public string Key => ReceiverType.Name;
}