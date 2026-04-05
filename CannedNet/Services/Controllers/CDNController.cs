using CannedNet.Services.Infrastructure;

namespace CannedNet.Services.Controllers;

public class CDNController
{
    public WebApplicationBuilder Initialize(string[]? args = null) => ServiceExtensions.CreateRecNetBuilder(args);

    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/config/LoadingScreenTipData", (HttpRequest request) =>
        {
            var json = File.ReadAllText("JSON/loadingscreentipdata.json");
            return Results.Content(json, "application/json");
        });
    }
}