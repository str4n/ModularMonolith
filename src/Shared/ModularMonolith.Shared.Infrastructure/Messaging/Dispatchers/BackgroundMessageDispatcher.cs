using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularMonolith.Shared.Abstractions.Modules;
using ModularMonolith.Shared.Infrastructure.Messaging.Channels;

namespace ModularMonolith.Shared.Infrastructure.Messaging.Dispatchers;

internal sealed class BackgroundMessageDispatcher : BackgroundService
{
    private readonly IMessageChannel _messageChannel;
    private readonly IModuleClient _moduleClient;
    private readonly ILogger<BackgroundMessageDispatcher> _logger;

    public BackgroundMessageDispatcher(IMessageChannel messageChannel, IModuleClient moduleClient, 
        ILogger<BackgroundMessageDispatcher> logger)
    {
        _messageChannel = messageChannel;
        _moduleClient = moduleClient;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Running the background message dispatcher.");
        
        while (true)
        {
            try
            {
                var message = _messageChannel.Read();

                if (message is null)
                {
                    continue;
                }
                
                _logger.LogInformation("Dispatching a message: {message}", message);

                await _moduleClient.PublishAsync(message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }
    }
}