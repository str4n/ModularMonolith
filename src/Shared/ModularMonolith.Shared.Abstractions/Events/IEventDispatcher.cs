namespace ModularMonolith.Shared.Abstractions.Events;

public interface IEventDispatcher
{ 
    Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
}