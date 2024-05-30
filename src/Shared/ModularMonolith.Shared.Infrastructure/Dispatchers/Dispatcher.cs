using ModularMonolith.Shared.Abstractions.Commands;
using ModularMonolith.Shared.Abstractions.Dispatchers;
using ModularMonolith.Shared.Abstractions.Queries;
using ModularMonolith.Shared.Infrastructure.Commands;
using ModularMonolith.Shared.Infrastructure.Queries;

namespace ModularMonolith.Shared.Infrastructure.Dispatchers;

internal sealed class Dispatcher : IDispatcher
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public Dispatcher(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        => await _commandDispatcher.DispatchAsync(command);

    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        => await _queryDispatcher.DispatchAsync<TQuery, TResult>(query);
}