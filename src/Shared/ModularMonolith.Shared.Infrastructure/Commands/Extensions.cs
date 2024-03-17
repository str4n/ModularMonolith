using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Shared.Abstractions.Commands;
using ModularMonolith.Shared.Infrastructure.Attributes;

namespace ModularMonolith.Shared.Infrastructure.Commands;

internal static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName!.Contains("ModularMonolith"));

        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)).WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

        return services;
    }
}