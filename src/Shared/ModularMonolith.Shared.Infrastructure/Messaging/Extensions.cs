using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Shared.Abstractions.Messaging;
using ModularMonolith.Shared.Infrastructure.Messaging.Brokers;
using ModularMonolith.Shared.Infrastructure.Messaging.Channels;
using ModularMonolith.Shared.Infrastructure.Messaging.Dispatchers;


namespace ModularMonolith.Shared.Infrastructure.Messaging;

internal static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
        services.AddSingleton<IMessageChannel, MessageChannel>();
        services.AddHostedService<BackgroundMessageDispatcher>();
        
        return services;
    }
}
