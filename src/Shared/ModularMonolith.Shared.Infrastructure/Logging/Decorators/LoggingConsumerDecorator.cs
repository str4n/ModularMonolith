using MassTransit;
using Microsoft.Extensions.Logging;
using ModularMonolith.Shared.Abstractions.Contexts;
using ModularMonolith.Shared.Abstractions.Messaging;
using ModularMonolith.Shared.Infrastructure.Attributes;
using ModularMonolith.Shared.Infrastructure.Modules;

namespace ModularMonolith.Shared.Infrastructure.Logging.Decorators;

[Decorator]
internal sealed class LoggingConsumerDecorator<TMessage> : IConsumer<TMessage> where TMessage : class, IMessage
{
    private readonly IConsumer<TMessage> _consumer;
    private readonly ILogger<LoggingConsumerDecorator<TMessage>> _logger;
    private readonly IContext _context;

    public LoggingConsumerDecorator(IConsumer<TMessage> consumer, ILogger<LoggingConsumerDecorator<TMessage>> logger,
        IContext context)
    {
        _consumer = consumer;
        _logger = logger;
        _context = context;
    }
    
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        var message = context.Message;
        
        var module = message.GetModuleName();
        var name = message.GetType().Name;
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity.Id;
        
        _logger.LogInformation("Consuming a message: {Name} ({Module}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, module, requestId, traceId, userId);

        await _consumer.Consume(context);
        
        _logger.LogInformation("Consumed a message: {Name} ({Module}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, module, requestId, traceId, userId);
    }
}