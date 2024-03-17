using MassTransit;
using Microsoft.Extensions.Logging;
using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Infrastructure.Messaging.Brokers;

internal sealed class RabbitMqMessageBroker : IMessageBroker
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RabbitMqMessageBroker> _logger;

    public RabbitMqMessageBroker(IPublishEndpoint publishEndpoint, ILogger<RabbitMqMessageBroker> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }
    
    public Task PublishAsync(params IMessage[] messages)
    {
        foreach (var message in messages)
        {
            _logger.LogInformation("Processing message: {message}", message);
            _publishEndpoint.Publish(message);
        }

        return Task.CompletedTask;
    }
}