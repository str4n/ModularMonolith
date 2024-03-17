using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Shared.Abstractions.Queries;

namespace ModularMonolith.Shared.Infrastructure.Queries;

internal static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName!.Contains("ModularMonolith"));
        
        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        return services;
    }
}