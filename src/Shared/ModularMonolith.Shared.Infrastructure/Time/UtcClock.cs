using ModularMonolith.Shared.Abstractions.Time;

namespace ModularMonolith.Shared.Infrastructure.Time;

internal sealed class UtcClock : IClock
{
    public DateTime Now() => DateTime.UtcNow;
}