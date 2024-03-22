namespace ModularMonolith.Shared.Infrastructure.Logging.Options;

public class LoggerOptions
{
    public string ConsoleOutputTemplate { get; set; }
    public bool EnableConsoleLogging { get; set; }
    public bool EnableSeqLogging { get; set; }
}