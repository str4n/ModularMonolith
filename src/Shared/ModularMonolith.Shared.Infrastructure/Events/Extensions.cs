using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Shared.Abstractions.Events;
using ModularMonolith.Shared.Infrastructure.Attributes;

namespace ModularMonolith.Shared.Infrastructure.Events;

internal static class Extensions
{
    // In memory event handling.
    
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName!.Contains("ModularMonolith"));
        
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))
                .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        
        return services;
    }
}