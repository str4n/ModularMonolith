using System.Collections.Concurrent;
using System.Threading.Channels;
using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Messaging.Channels;

internal sealed class MessageChannel : IMessageChannel
{
    private static readonly ConcurrentQueue<IMessage> Messages = new();

    public void Publish(IMessage message)
        => Messages.Enqueue(message);

    public IMessage Read()
    {
        Messages.TryDequeue(out var message);

        return message;
    }
}