using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Shared.Infrastructure.Contexts.Factory;

namespace ModularMonolith.Shared.Infrastructure.Contexts;

internal static class Extensions
{
    private const string CorrelationIdKey = "correlation-id";

    public static IServiceCollection AddContext(this IServiceCollection services)
    {
        services.AddSingleton<IContextFactory, ContextFactory>();
        services.AddTransient(sp => sp.GetRequiredService<IContextFactory>().Create());

        return services;
    }
    
    public static Guid? TryGetCorrelationId(this HttpContext context)
        => context.Items.TryGetValue(CorrelationIdKey, out var id) ? (Guid) id : null;
    
    public static string GetUserIpAddress(this HttpContext context)
    {
        if (context is null)
        {
            return string.Empty;
        }
            
        var ipAddress = context.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        if (context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
        {
            var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (ipAddresses.Any())
            {
                ipAddress = IPAddress.Parse(ipAddresses[0]).MapToIPv4().ToString();
            }
        }

        return ipAddress ?? string.Empty;
    }
}