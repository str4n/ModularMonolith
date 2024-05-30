using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Shared.Abstractions.Queries;

namespace ModularMonolith.Shared.Infrastructure.Queries;

internal sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
    {
        if (query is null)
        {
            return default;
        }

        using var scope = _serviceProvider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

        return await handler.HandleAsync(query);
    }
}