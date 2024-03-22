using ModularMonolith.Shared.Abstractions.Contexts;

namespace ModularMonolith.Shared.Infrastructure.Contexts.Factory;

internal interface IContextFactory
{
    IContext Create();
}