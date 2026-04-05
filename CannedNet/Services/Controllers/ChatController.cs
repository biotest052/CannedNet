using CannedNet.Services.Infrastructure;

namespace CannedNet.Services.Controllers;

public class ChatController
{
    public WebApplicationBuilder Initialize(string[]? args = null) => ServiceExtensions.CreateRecNetBuilder(args);

    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/thread", () => Results.Content("[]", "application/json"));
    }
}
