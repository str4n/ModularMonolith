using ModularMonolith.Bootstrapper;
using ModularMonolith.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Load modules here
// ModuleLoader.Load<SampleModule>();

builder.Services
    .AddModules(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseModules();

app.Run();