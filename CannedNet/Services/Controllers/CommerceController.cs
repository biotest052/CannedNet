using CannedNet.Services.Infrastructure;

namespace CannedNet.Services.Controllers;

public class CommerceController
{
    public WebApplicationBuilder Initialize(string[]? args = null) => ServiceExtensions.CreateRecNetBuilder(args);

    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/purchase/v1/hasspentmoney", (HttpRequest request) =>
        {
            return Results.NotFound();
        });
    }
}