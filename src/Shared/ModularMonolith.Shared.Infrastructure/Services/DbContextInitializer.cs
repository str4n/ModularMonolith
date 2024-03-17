using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ModularMonolith.Shared.Infrastructure.Services;

public class DbContextInitializer : IInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DbContextInitializer> _logger;

    public DbContextInitializer(IServiceProvider serviceProvider, ILogger<DbContextInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InitAsync()
    {
        var dbContextTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsAssignableTo(typeof(DbContext)) && !x.IsInterface && x != typeof(DbContext));
        
        using var scope = _serviceProvider.CreateScope();
        
        foreach (var dbContextType in dbContextTypes)
        {
            var dbContext = scope.ServiceProvider.GetService(dbContextType) as DbContext;

            if (dbContext is not null)
            {
                await dbContext.Database.MigrateAsync();
                _logger.LogInformation("{dbContext} initialized.", dbContext.GetType());
            }
        }
    }
}