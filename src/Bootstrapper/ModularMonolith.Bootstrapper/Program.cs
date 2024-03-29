using ModularMonolith.Bootstrapper;
using ModularMonolith.Shared.Abstractions.Contexts;
using ModularMonolith.Shared.Infrastructure;
using ModularMonolith.Shared.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLogging(builder.Configuration);

// Load modules here
// ModuleLoader.Load<SampleModule>();

builder.Services
    .AddModules(builder.Configuration)
    .AddInfrastructure(builder.Configuration, ModuleLoader.GetModules());

var app = builder.Build();

app.UseModules();

app.UseInfrastructure();

app.Run();