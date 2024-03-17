﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ModularMonolith.Shared.Abstractions.Time;
using ModularMonolith.Shared.Infrastructure.Commands;
using ModularMonolith.Shared.Infrastructure.Events;
using ModularMonolith.Shared.Infrastructure.Queries;
using ModularMonolith.Shared.Infrastructure.Time;

namespace ModularMonolith.Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddHttpContextAccessor();

        services.AddControllers();
        
        services.AddSingleton<IClock, UtcClock>();
        
        services.AddSwaggerGen(swagger =>
        {
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ModularMonolith API",
                Version = "v1"
            });
        });
        
        services
            .AddCommands()
            .AddQueries();

        // services.AddEvents();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseSwagger();
        app.UseReDoc(options =>
        {
            options.RoutePrefix = "docs";
            options.DocumentTitle = "ModularMonolith API";
            options.SpecUrl("/swagger/v1/swagger.json");
        });
        
        app.UseAuthentication();
        
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        return app;
    }
    
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);

        return options;
    }
}