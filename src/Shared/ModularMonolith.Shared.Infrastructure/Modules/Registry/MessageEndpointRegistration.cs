using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Modules.Registry;

internal sealed record MessageEndpointRegistration(Type ReceiverType, Func<IMessage, Task> HandleAsync)
{
    public string Key => ReceiverType.Name;
}