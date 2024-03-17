using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Shared.Infrastructure.Services;

internal static class Extensions
{
    public static IServiceCollection AddInitializers(this IServiceCollection services)
    {
        services.AddSingleton<IInitializer, DbContextInitializer>();
        
        services.AddHostedService<AppInitializer>();

        return services;
    }
}