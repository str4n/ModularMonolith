using ModularMonolith.Shared.Abstractions.Commands;

namespace ModularMonolith.Shared.Infrastructure.Commands;

internal interface ICommandDispatcher
{
    Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
}