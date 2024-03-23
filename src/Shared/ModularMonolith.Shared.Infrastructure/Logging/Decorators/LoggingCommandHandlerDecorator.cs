using Microsoft.Extensions.Logging;
using ModularMonolith.Shared.Abstractions.Commands;
using ModularMonolith.Shared.Abstractions.Contexts;
using ModularMonolith.Shared.Infrastructure.Attributes;
using ModularMonolith.Shared.Infrastructure.Modules;

namespace ModularMonolith.Shared.Infrastructure.Logging.Decorators;

[Decorator]
internal sealed class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand>> _logger;
    private readonly IContext _context;

    public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> handler, ILogger<LoggingCommandHandlerDecorator<TCommand>> logger,
        IContext context)
    {
        _handler = handler;
        _logger = logger;
        _context = context;
    }
    
    public async Task HandleAsync(TCommand command)
    {
        var module = command.GetModuleName();
        var name = command.GetType().Name;
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity.Id;
        
        _logger.LogInformation("Handling a command: {Name} ({Module}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, module, requestId, traceId, userId);

        await _handler.HandleAsync(command);
        
        _logger.LogInformation("Handled a command: {Name} ({Module}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, module, requestId, traceId, userId);
    }
}