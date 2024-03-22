using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
}