using ModularMonolith.Shared.Abstractions.Queries;

namespace ModularMonolith.Shared.Infrastructure.Queries;

internal interface IQueryDispatcher
{
    Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
}