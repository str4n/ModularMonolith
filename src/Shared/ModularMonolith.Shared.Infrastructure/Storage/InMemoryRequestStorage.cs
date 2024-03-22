using Microsoft.Extensions.Caching.Memory;
using ModularMonolith.Shared.Abstractions.Storage;

namespace ModularMonolith.Shared.Infrastructure.Storage;

internal sealed class InMemoryRequestStorage : IRequestStorage
{
    private readonly IMemoryCache _cache;

    public InMemoryRequestStorage(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    public void Set<T>(string key, T value, TimeSpan? duration = null) 
        => _cache.Set(key, value, duration ?? TimeSpan.FromSeconds(5));

    public T Get<T>(string key) => _cache.Get<T>(key);
}