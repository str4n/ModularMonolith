namespace ModularMonolith.Shared.Abstractions.Messaging;

public interface IMessageDispatcher
{
    Task PublishAsync(IMessage message);
}