using System.Threading.Channels;
using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Messaging.Channels;

internal interface IMessageChannel
{
    void Publish(IMessage message);
    IMessage Read();
}