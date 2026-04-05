using CannedNet.Hubs;
using CannedNet.Services.Infrastructure;

namespace CannedNet.Services.Controllers;

public class NSController
{
    public WebApplicationBuilder Initialize(string[]? args = null) => ServiceExtensions.CreateRecNetBuilder(args);

    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/", () =>
        {
            var json = File.ReadAllText("JSON/endpoints.json");
            return Results.Content(json, "application/json");
        });
        
        app.MapGet("/photon", () =>
        {
            var json = File.ReadAllText("JSON/photonsettings.json");
            return Results.Content(json, "application/json");
        });
    }
}
