using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Shared.Abstractions.Cache;

namespace ModularMonolith.Shared.Infrastructure.Cache;

internal static class Extensions
{
    private const string SectionName = "Redis";
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<RedisOptions>(SectionName);
        services.AddStackExchangeRedisCache(r =>
        {
            r.Configuration = options.ConnectionString;
        });

        services.AddScoped<ICache, RedisCache>();

        return services;
    }
}