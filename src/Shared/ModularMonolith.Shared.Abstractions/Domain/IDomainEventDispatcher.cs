namespace ModularMonolith.Shared.Abstractions.Domain;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent @event);
}