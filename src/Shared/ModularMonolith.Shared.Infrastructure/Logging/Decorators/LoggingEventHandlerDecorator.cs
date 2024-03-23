using Microsoft.Extensions.Logging;
using ModularMonolith.Shared.Abstractions.Commands;
using ModularMonolith.Shared.Abstractions.Contexts;
using ModularMonolith.Shared.Abstractions.Events;
using ModularMonolith.Shared.Infrastructure.Attributes;
using ModularMonolith.Shared.Infrastructure.Modules;

namespace ModularMonolith.Shared.Infrastructure.Logging.Decorators;

[Decorator]
internal sealed class LoggingEventHandlerDecorator<TEvent> : IEventHandler<TEvent> where TEvent : class, IEvent
{
    private readonly IEventHandler<TEvent> _handler;
    private readonly ILogger<LoggingEventHandlerDecorator<TEvent>> _logger;
    private readonly IContext _context;

    public LoggingEventHandlerDecorator(IEventHandler<TEvent> handler, ILogger<LoggingEventHandlerDecorator<TEvent>> logger,
        IContext context)
    {
        _handler = handler;
        _logger = logger;
        _context = context;
    }
    
    public async Task HandleAsync(TEvent @event)
    {
        var module = @event.GetModuleName();
        var name = @event.GetType().Name;
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity.Id;
        
        _logger.LogInformation("Handling an event: {Name} ({Module}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, module, requestId, traceId, userId);

        await _handler.HandleAsync(@event);
        
        _logger.LogInformation("Handled an event: {Name} ({Module}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, module, requestId, traceId, userId);
    }
}