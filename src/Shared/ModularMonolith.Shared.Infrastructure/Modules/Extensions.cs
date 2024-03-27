using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Shared.Abstractions.Commands;
using ModularMonolith.Shared.Abstractions.Events;
using ModularMonolith.Shared.Abstractions.Messaging;
using ModularMonolith.Shared.Abstractions.Modules;
using ModularMonolith.Shared.Infrastructure.Commands;
using ModularMonolith.Shared.Infrastructure.Modules.Client;
using ModularMonolith.Shared.Infrastructure.Modules.Registry;
using ModularMonolith.Shared.Infrastructure.Modules.Serialization;

namespace ModularMonolith.Shared.Infrastructure.Modules;

internal static class Extensions
{
    private const string NamespacePart = "Modules";

    public static IServiceCollection AddModuleMessageEndpoints(this IServiceCollection services, IEnumerable<Module> modules)
    {
        services.AddModuleRegistry(modules);

        services.AddSingleton<IModuleClient, ModuleClient>();
        services.AddSingleton<IModuleSerializer, JsonModuleSerializer>();

        return services;
    }

    private static IServiceCollection AddModuleRegistry(this IServiceCollection services, IEnumerable<Module> modules)
    {
        var registry = new ModuleRegistry();

        var types =
            modules.Select(x => x.GetType().Assembly)
                .SelectMany(x => x.GetTypes()).ToArray();
        
        // Messages
        
        var eventTypes = types
            .Where(x => x.IsClass && x.IsAssignableTo(typeof(IEvent)))
            .ToArray();
        
        var commandTypes = types
            .Where(x => x.IsClass && x.IsAssignableTo(typeof(ICommand)))
            .ToArray();

        services.AddSingleton<IModuleRegistry>(sp =>
        {
            var eventDispatcher = sp.GetRequiredService<IEventDispatcher>();
            var eventDispatcherType = eventDispatcher.GetType();
            
            var commandDispatcher = sp.GetRequiredService<ICommandDispatcher>();
            var commandDispatcherType = commandDispatcher.GetType();
            
            foreach (var type in eventTypes)
            {
                registry.AddMessageRegistration(type, @event =>
                    (Task) eventDispatcherType.GetMethod(nameof(eventDispatcher.PublishAsync))
                        ?.MakeGenericMethod(type)
                        .Invoke(eventDispatcher, new[] { @event }));
            }
            
            foreach (var type in commandTypes)
            {
                registry.AddMessageRegistration(type, command =>
                    (Task) commandDispatcherType.GetMethod(nameof(commandDispatcher.SendAsync))
                        ?.MakeGenericMethod(type)
                        .Invoke(commandDispatcher, new[] { command }));
            }
        
            return registry;
        });

        return services;
    }
    
    public static string GetModuleName(this object value)
    {
        if (value?.GetType() is null)
        {
            return string.Empty;
        }

        var type = value.GetType();
        
        if (type.Namespace is null)
        {
            return string.Empty;
        }

        return type.Namespace.Contains(NamespacePart) ? type.Namespace.Split(".")[2] : string.Empty;
    }
}