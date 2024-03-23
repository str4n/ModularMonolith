using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularMonolith.Shared.Abstractions.Commands;
using ModularMonolith.Shared.Abstractions.Contexts;
using ModularMonolith.Shared.Abstractions.Events;
using ModularMonolith.Shared.Abstractions.Queries;
using ModularMonolith.Shared.Infrastructure.Logging.Decorators;
using ModularMonolith.Shared.Infrastructure.Logging.Options;
using Serilog;
using Serilog.Events;

namespace ModularMonolith.Shared.Infrastructure.Logging;

public static class Extensions
{
    private const string LoggerSectionName = "Logger";
    private const string SeqSectionName = "Seq";
    public static IHostBuilder UseLogging(this IHostBuilder host, IConfiguration configuration)
    {
        var loggerOptions = configuration.GetOptions<LoggerOptions>(LoggerSectionName);
        var seqOptions = configuration.GetOptions<SeqOptions>(SeqSectionName);
        
        
        host.UseSerilog((ctx, cfg) =>
        {
            if (loggerOptions.EnableConsoleLogging)
            {
                cfg.WriteTo.Console(outputTemplate: loggerOptions.ConsoleOutputTemplate);
            }

            if (loggerOptions.EnableSeqLogging)
            {
                cfg.WriteTo.Seq(seqOptions.ConnectionString);
            }
        });

        return host;
    }

    internal static IServiceCollection AddLoggingDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
        services.TryDecorate(typeof(IEventHandler<>), typeof(LoggingEventHandlerDecorator<>));
        services.TryDecorate(typeof(IConsumer<>), typeof(LoggingConsumerDecorator<>));

        return services;
    }
    
    internal static WebApplication UseLogging(this WebApplication app)
    {
        app.Use(async (ctx, next) =>
        {
            var logger = ctx.RequestServices.GetRequiredService<ILogger<IContext>>();
            var context = ctx.RequestServices.GetRequiredService<IContext>();
            logger.LogInformation("Started processing a request [Request ID: '{RequestId}', Correlation ID: '{CorrelationId}', Trace ID: '{TraceId}', User ID: '{UserId}']...",
                context.RequestId, context.CorrelationId, context.TraceId, context.Identity.IsAuthenticated ? context.Identity.Id : string.Empty);
                
            await next();
                
            logger.LogInformation("Finished processing a request with status code: {StatusCode} [Request ID: '{RequestId}', Correlation ID: '{CorrelationId}', Trace ID: '{TraceId}', User ID: '{UserId}']",
                ctx.Response.StatusCode, context.RequestId, context.CorrelationId, context.TraceId, context.Identity.IsAuthenticated ? context.Identity.Id : string.Empty);
        });

        return app;
    }
}