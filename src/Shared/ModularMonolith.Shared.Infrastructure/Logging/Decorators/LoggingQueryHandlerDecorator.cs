using Microsoft.Extensions.Logging;
using ModularMonolith.Shared.Abstractions.Contexts;
using ModularMonolith.Shared.Abstractions.Queries;
using ModularMonolith.Shared.Infrastructure.Attributes;
using ModularMonolith.Shared.Infrastructure.Modules;

namespace ModularMonolith.Shared.Infrastructure.Logging.Decorators;

[Decorator]
internal sealed class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _handler;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> _logger;
    private readonly IContext _context;

    public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> handler, ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger,
        IContext context)
    {
        _handler = handler;
        _logger = logger;
        _context = context;
    }

    public async Task<TResult> HandleAsync(TQuery query)
    {
        var module = query.GetModuleName();
        var name = query.GetType().Name;
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity.Id;
        
        _logger.LogInformation("Handling a query: {Name} ({Module}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, module, requestId, traceId, userId);

        var result = await _handler.HandleAsync(query);
        
        _logger.LogInformation("Handled a query: {Name} ({Module}) [Request ID: {RequestId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, module, requestId, traceId, userId);

        return result;
    }
}