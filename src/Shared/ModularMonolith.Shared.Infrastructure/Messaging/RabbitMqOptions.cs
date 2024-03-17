namespace ModularMonolith.Shared.Infrastructure.Messaging;

internal sealed class RabbitMqOptions
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}