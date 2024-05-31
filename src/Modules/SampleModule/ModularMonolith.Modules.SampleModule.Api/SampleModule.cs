using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Modules.SampleModule.Core;
using ModularMonolith.Shared.Abstractions.Modules;

namespace ModularMonolith.Modules.SampleModule.Api;

public sealed class SampleModule : Module
{
    public string BasePath => "sample-module";
    public override string Path => BasePath;
    public override void AddModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
    }

    public override void UseModule(WebApplication app)
    {
        //
    }
}