using Microsoft.Extensions.Hosting;
using Serilog;

namespace ModularMonolith.Shared.Infrastructure.Logging;

public static class Extensions
{
    public static IHostBuilder UseLogging(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            configuration.WriteTo.Console();
        });

        return host;
    }
}