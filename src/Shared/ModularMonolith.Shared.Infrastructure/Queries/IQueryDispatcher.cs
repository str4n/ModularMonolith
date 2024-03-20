using ModularMonolith.Shared.Abstractions.Queries;

namespace ModularMonolith.Shared.Infrastructure.Queries;

internal interface IQueryDispatcher
{
    Task<TResult> SendAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
}