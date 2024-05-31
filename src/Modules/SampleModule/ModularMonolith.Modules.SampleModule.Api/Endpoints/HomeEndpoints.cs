using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace ModularMonolith.Modules.SampleModule.Api.Endpoints;

internal static class HomeEndpoints
{
    public static IEndpointRouteBuilder MapHomeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => "Hello World!");

        return app;
    }
    
}