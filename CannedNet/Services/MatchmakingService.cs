using System.Text.Json;
using System.Text.Json.Serialization;
using CannedNet.Data;
using Microsoft.EntityFrameworkCore;

namespace CannedNet.Services;

public class MatchmakingService
{
    public WebApplicationBuilder Initialize(string[]? args = null) => ServiceExtensions.CreateRecNetBuilder(args);

    public void MapEndpoints(WebApplication app)
    {
        var jwtService = app.Services.GetRequiredService<JwtTokenService>();
        
        app.MapPost("/player/login", () => Results.Ok());
        
        app.MapGet("/player", (HttpRequest request) =>
        {
            /*
            var id = request.Query["id"];
            var accounts = new List<Account>();

            if (int.TryParse(id, out var accountId))
            {
                accounts.Add(new Account
                {
                    AccountId = accountId,
                    ProfileImage = "hdqeamlcmatc6qzoi2ybgf0ddijjcf.jpg",
                    IsJunior = false,
                    Platforms = 0,
                    PersonalPronouns = 0,
                    IdentityFlags = 0,
                    Username = $"Player{accountId}",
                    DisplayName = $"Player{accountId}",
                    CreatedAt = DateTime.UtcNow
                });
            }

            return Results.Json(accounts);*/
        
            var json = File.ReadAllText("JSON/getplayer.json");
            return Results.Content(json, "application/json");
        });

        app.MapPost("/goto/room/{room}", async (HttpRequest request, string room, AppDbContext db) =>
        {
            var authHeader = request.Headers.Authorization.ToString();
            
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return Results.Unauthorized();

            var token = authHeader.Substring("Bearer ".Length);
            var accountId = jwtService.ValidateAndGetAccountId(token);

            if (string.IsNullOrEmpty(accountId))
                return Results.Unauthorized();

            if (!int.TryParse(accountId.AsSpan(), out var id))
                return Results.Unauthorized();

            var photonRoomId = Guid.NewGuid().ToString();
            
            var existingInstance = await db.RoomInstances.FirstOrDefaultAsync(r => r.OwnerAccountId == id);
            
            if (existingInstance != null)
            {
                existingInstance.roomInstanceId = 1;
                existingInstance.roomId = 1;
                existingInstance.subRoomId = 1;
                existingInstance.roomInstanceType = 2;
                existingInstance.location = "76d98498-60a1-430c-ab76-b54a29b7a163";
                existingInstance.dataBlob = "";
                existingInstance.eventId = 0;
                existingInstance.clubId = 0;
                existingInstance.roomCode = "";
                existingInstance.photonRegionId = "us";
                existingInstance.photonRoomId = photonRoomId;
                existingInstance.name = room;
                existingInstance.maxCapacity = 4;
                existingInstance.isFull = false;
                existingInstance.isPrivate = true;
                existingInstance.isInProgress = false;
                existingInstance.EncryptVoiceChat = false;
            }
            else
            {
                db.RoomInstances.Add(new RoomInstance
                {
                    OwnerAccountId = id,
                    roomInstanceId = 1,
                    roomId = 1,
                    subRoomId = 1,
                    roomInstanceType = 2,
                    location = "76d98498-60a1-430c-ab76-b54a29b7a163",
                    dataBlob = "",
                    eventId = 0,
                    clubId = 0,
                    roomCode = "",
                    photonRegionId = "us",
                    photonRoomId = photonRoomId,
                    name = room,
                    maxCapacity = 4,
                    isFull = false,
                    isPrivate = true,
                    isInProgress = false,
                    EncryptVoiceChat = false
                });
            }
            
            await db.SaveChangesAsync();
            
            return Results.Json(new
            {
                errorCode = 0,
                roomInstance = new
                {
                    roomInstanceId = 1,
                    roomId = 1,
                    subRoomId = 1,
                    roomInstanceType = 2,
                    location = "76d98498-60a1-430c-ab76-b54a29b7a163",
                    dataBlob = "",
                    eventId = 0,
                    clubId = 0,
                    roomCode = "",
                    photonRegionId = "us",
                    photonRoomId = photonRoomId,
                    name = room,
                    maxCapacity = 4,
                    isFull = false,
                    isPrivate = true,
                    isInProgress = false,
                    EncryptVoiceChat = false
                }
            });
        });
        
        app.MapPost("/goto/none", (HttpRequest request) =>
        {
            // dormroom location id : 76d98498-60a1-430c-ab76-b54a29b7a163
            // offline dorm
            
            return Results.Json(new
            {
                errorCode = 0,
                roomInstance = new
                {
                    roomInstanceId = 1,
                    roomId = 1,
                    subRoomId = 1,
                    roomInstanceType = 2,
                    location = "76d98498-60a1-430c-ab76-b54a29b7a163",
                    dataBlob = "",
                    eventId = 0,
                    clubId = 0,
                    photonRegionId = "us",
                    photonRoomId = Guid.NewGuid(),
                    name = "DormRoom",
                    maxCapacity = 4,
                    isFull = false,
                    isPrivate = true,
                    isInProgress = false,
                    EncryptVoiceChat = false
                }
            });
        });

        app.MapPost("/player/heartbeat", async (HttpRequest request, AppDbContext db) =>
        {
            var authHeader = request.Headers.Authorization.ToString();
            
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return Results.Unauthorized();

            var token = authHeader.Substring("Bearer ".Length);
            var accountId = jwtService.ValidateAndGetAccountId(token);

            if (string.IsNullOrEmpty(accountId))
                return Results.Unauthorized();

            if (!int.TryParse(accountId.AsSpan(), out var id))
                return Results.Unauthorized();

            request.EnableBuffering();
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            
            request.Body.Position = 0;
            var form = await request.ReadFormAsync();
            var loginLock = form["LoginLock"].ToString();
            
            HeartbeatRequest? heartbeat = null;
            if (!string.IsNullOrWhiteSpace(body) && body.TrimStart().StartsWith("{"))
            {
                heartbeat = JsonSerializer.Deserialize<HeartbeatRequest>(body);
            }
            
            if (heartbeat == null)
                heartbeat = new HeartbeatRequest();

            var roomInstance = await db.RoomInstances.FirstOrDefaultAsync(r => r.OwnerAccountId == id);

            return Results.Json(new
            {
                playerId = id,
                statusVisibility = heartbeat.statusVisibility,
                deviceClass = heartbeat.deviceClass,
                vrMovementMode = heartbeat.vrMovementMode,
                roomInstance = new
                {
                    roomInstanceId = roomInstance?.roomInstanceId ?? 0,
                    roomId = roomInstance?.roomId ?? 0,
                    subRoomId = roomInstance?.subRoomId ?? 0,
                    roomInstanceType = roomInstance?.roomInstanceType ?? 0,
                    location = roomInstance?.location ?? "",
                    dataBlob = roomInstance?.dataBlob ?? "",
                    eventId = roomInstance?.eventId ?? 0,
                    clubId = roomInstance?.clubId ?? 0,
                    roomCode = roomInstance?.roomCode ?? "",
                    photonRegionId = roomInstance?.photonRegionId ?? "us",
                    photonRoomId = roomInstance?.photonRoomId ?? "",
                    name = roomInstance?.name ?? "",
                    maxCapacity = roomInstance?.maxCapacity ?? 0,
                    isFull = roomInstance?.isFull ?? false,
                    isPrivate = roomInstance?.isPrivate ?? false,
                    isInProgress = roomInstance?.isInProgress ?? false,
                    EncryptVoiceChat = roomInstance?.EncryptVoiceChat ?? false
                },
                isOnline = true,
                appVersion = heartbeat.appVersion ?? "",
                platform = heartbeat.platform
            });
        });
        app.MapPut("/player/statusvisibility", async (HttpRequest request, AppDbContext db) =>
        {
            // TODO ADD FUNCTIONALITY
            return Results.Ok();
        });
    }
}

public class HeartbeatRequest
{
    [JsonPropertyName("playerId")]
    public int playerId { get; set; }

    [JsonPropertyName("statusVisibility")]
    public int statusVisibility { get; set; }

    [JsonPropertyName("deviceClass")]
    public int deviceClass { get; set; }

    [JsonPropertyName("vrMovementMode")]
    public int vrMovementMode { get; set; }

    [JsonPropertyName("roomInstance")]
    public RoomInstanceInfo? roomInstance { get; set; }

    [JsonPropertyName("isOnline")]
    public bool isOnline { get; set; }

    [JsonPropertyName("appVersion")]
    public string? appVersion { get; set; }

    [JsonPropertyName("platform")]
    public int platform { get; set; }
}

public class RoomInstanceInfo
{
    [JsonPropertyName("roomInstanceId")]
    public int roomInstanceId { get; set; }

    [JsonPropertyName("roomId")]
    public int roomId { get; set; }

    [JsonPropertyName("subRoomId")]
    public int subRoomId { get; set; }

    [JsonPropertyName("roomInstanceType")]
    public int roomInstanceType { get; set; }

    [JsonPropertyName("location")]
    public string? location { get; set; }

    [JsonPropertyName("dataBlob")]
    public string? dataBlob { get; set; }

    [JsonPropertyName("eventId")]
    public int eventId { get; set; }

    [JsonPropertyName("clubId")]
    public int clubId { get; set; }

    [JsonPropertyName("roomCode")]
    public string? roomCode { get; set; }

    [JsonPropertyName("photonRegionId")]
    public string? photonRegionId { get; set; }

    [JsonPropertyName("photonRoomId")]
    public string? photonRoomId { get; set; }

    [JsonPropertyName("name")]
    public string? name { get; set; }

    [JsonPropertyName("maxCapacity")]
    public int maxCapacity { get; set; }

    [JsonPropertyName("isFull")]
    public bool isFull { get; set; }

    [JsonPropertyName("isPrivate")]
    public bool isPrivate { get; set; }

    [JsonPropertyName("isInProgress")]
    public bool isInProgress { get; set; }

    [JsonPropertyName("EncryptVoiceChat")]
    public bool EncryptVoiceChat { get; set; }
}
