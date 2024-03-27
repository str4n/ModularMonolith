using System.ComponentModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularMonolith.Shared.Abstractions.Modules;
using ModularMonolith.Shared.Infrastructure.Messaging.Channels;

namespace ModularMonolith.Shared.Infrastructure.Messaging.Dispatchers;

internal sealed class BackgroundMessageDispatcher : IHostedService
{
    private readonly IMessageChannel _messageChannel;
    private readonly IModuleClient _moduleClient;
    private readonly ILogger<BackgroundMessageDispatcher> _logger;
    private Timer _timer;

    public BackgroundMessageDispatcher(IMessageChannel messageChannel, IModuleClient moduleClient, 
        ILogger<BackgroundMessageDispatcher> logger)
    {
        _messageChannel = messageChannel;
        _moduleClient = moduleClient;
        _logger = logger;
    }

    private async void DoWork(object state)
    {
        try
        {
            var message = _messageChannel.Read();

            if (message is null)
            {
                return;
            }
                
            _logger.LogInformation("Dispatching a message: {message}", message);

            await _moduleClient.PublishAsync(message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Running the background message dispatcher.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopped the background message dispatcher.");

        return Task.CompletedTask;
    }
}