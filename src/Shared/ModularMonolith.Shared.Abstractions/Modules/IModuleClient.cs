using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Abstractions.Modules;

public interface IModuleClient
{
    Task PublishAsync(IMessage message);
}