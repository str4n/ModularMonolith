using ModularMonolith.Shared.Abstractions.Messaging;

namespace ModularMonolith.Shared.Abstractions.Queries;

public interface IQuery : IMessage
{
}

public interface IQuery<TResult> : IQuery
{
}