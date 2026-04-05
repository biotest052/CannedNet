using CannedNet.Services.Infrastructure;

namespace CannedNet.Services.Controllers;

public class ClubsController
{
    public WebApplicationBuilder Initialize(string[]? args = null) => ServiceExtensions.CreateRecNetBuilder(args);

    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/club/home/me", () =>
        {
            return Results.NotFound();
        });
    }
}