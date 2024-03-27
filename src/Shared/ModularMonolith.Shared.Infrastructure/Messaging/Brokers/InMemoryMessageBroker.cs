using ModularMonolith.Shared.Abstractions.Messaging;
using ModularMonolith.Shared.Abstractions.Modules;
using ModularMonolith.Shared.Infrastructure.Messaging.Channels;

namespace ModularMonolith.Shared.Infrastructure.Messaging.Brokers;

internal sealed class InMemoryMessageBroker : IMessageBroker
{
    private readonly IMessageChannel _messageChannel;

    public InMemoryMessageBroker(IMessageChannel messageChannel)
    {
        _messageChannel = messageChannel;
    }

    public Task PublishAsync(params IMessage[] messages)
    {
        if (messages is null)
        {
            return Task.CompletedTask;
        }

        messages = messages.Where(x => x is not null).ToArray();

        foreach (var message in messages)
        {
            _messageChannel.Publish(message);
        }
        
        return Task.CompletedTask;
    }
}