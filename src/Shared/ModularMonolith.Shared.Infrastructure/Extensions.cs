﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ModularMonolith.Shared.Abstractions.Dispatchers;
using ModularMonolith.Shared.Abstractions.Modules;
using ModularMonolith.Shared.Abstractions.Serialization;
using ModularMonolith.Shared.Abstractions.Storage;
using ModularMonolith.Shared.Abstractions.Time;
using ModularMonolith.Shared.Infrastructure.Auth;
using ModularMonolith.Shared.Infrastructure.Cache;
using ModularMonolith.Shared.Infrastructure.Commands;
using ModularMonolith.Shared.Infrastructure.Contexts;
using ModularMonolith.Shared.Infrastructure.Dispatchers;
using ModularMonolith.Shared.Infrastructure.Events;
using ModularMonolith.Shared.Infrastructure.Exceptions;
using ModularMonolith.Shared.Infrastructure.Logging;
using ModularMonolith.Shared.Infrastructure.Messaging;
using ModularMonolith.Shared.Infrastructure.Modules;
using ModularMonolith.Shared.Infrastructure.Postgres;
using ModularMonolith.Shared.Infrastructure.Queries;
using ModularMonolith.Shared.Infrastructure.Serialization;
using ModularMonolith.Shared.Infrastructure.Services;
using ModularMonolith.Shared.Infrastructure.Storage;
using ModularMonolith.Shared.Infrastructure.Time;

namespace ModularMonolith.Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, 
        IEnumerable<Module> modules)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ModularMonolith API",
                Version = "v1"
            });
        });
        
        services.AddExceptionHandling();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddContext();
        services.AddInitializers();
        services.AddAuth(configuration);
        services.AddMemoryCache();
        services.AddCaching(configuration);
        services.AddMessaging(configuration);
        services.AddModuleMessageEndpoints(modules);
        
        services
            .AddCommands()
            .AddQueries()
            .AddEvents();
        
        services.AddLoggingDecorators();
        
        services.ConfigurePostgres(configuration);
        
        services.AddSingleton<IClock, UtcClock>();
        services.AddSingleton<IJsonSerializer, NewtonsoftSerializer>();
        services.AddSingleton<IRequestStorage, InMemoryRequestStorage>();
        services.AddSingleton<IDispatcher, Dispatcher>();
        

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseExceptionHandling();
        
        app.UseSwagger();
        app.UseReDoc(options =>
        {
            options.RoutePrefix = "docs";
            options.DocumentTitle = "ModularMonolith API";
            options.SpecUrl("/swagger/v1/swagger.json");
        });
        
        app.UseAuthentication();
        app.UseHttpsRedirection();
        app.UseLogging();
        app.UseRouting();
        app.UseAuthorization();
        
        app.MapControllers();

        app.MapGet("/", () => "Hello");

        return app;
    }
    
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);

        return options;
    }
}