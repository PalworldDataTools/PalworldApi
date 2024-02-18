using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PalworldApi.OpenApi;
using PalworldApi.Services;
using PalworldDataExtractor.Models.Steam;

namespace PalworldApi.v1.Controllers;

public class PalworldSteamApplicationEndpoints
{
    const string Tags = "Steam application";

    readonly RawDataService _rawDataService;

    public PalworldSteamApplicationEndpoints(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    public Results<Ok<string>, NotFound> GetSteamApplicationId() => TryGetManifest(out SteamManifest? manifest) ? TypedResults.Ok(manifest.AppId) : ManifestNotFound();
    public Results<Ok<string>, NotFound> GetSteamApplicationName() => TryGetManifest(out SteamManifest? manifest) ? TypedResults.Ok(manifest.AppName) : ManifestNotFound();
    public Results<Ok<string>, NotFound> GetSteamBuildId() => TryGetManifest(out SteamManifest? manifest) ? TypedResults.Ok(manifest.BuildId) : ManifestNotFound();
    public Results<Ok<long>, NotFound> GetSteamApplicationSize() => TryGetManifest(out SteamManifest? manifest) ? TypedResults.Ok(manifest.AppSize) : ManifestNotFound();

    bool TryGetManifest([NotNullWhen(true)] out SteamManifest? steamManifest)
    {
        steamManifest = _rawDataService.Data.SteamManifest;
        return steamManifest != null;
    }

    static NotFound ManifestNotFound() => TypedResults.NotFound();

    public static void Map(WebApplication app, string? routePrefix = null)
    {
        app.MapGet("v1/application/steam/id", ([FromServices] PalworldSteamApplicationEndpoints endpoints) => endpoints.GetSteamApplicationId())
            .WithVersion("v1")
            .WithTags(Tags)
            .WithName(nameof(GetSteamApplicationId))
            .WithSummary("Get steam application id")
            .WithDescription("Get the application ID of the steam version of the game.");

        app.MapGet("v1/application/steam/name", ([FromServices] PalworldSteamApplicationEndpoints endpoints) => endpoints.GetSteamApplicationName())
            .WithVersion("v1")
            .WithTags(Tags)
            .WithName(nameof(GetSteamApplicationName))
            .WithSummary("Get steam application name")
            .WithDescription("Get the name of the steam version of the game.");

        app.MapGet("v1/application/steam/build-id", ([FromServices] PalworldSteamApplicationEndpoints endpoints) => endpoints.GetSteamBuildId())
            .WithVersion("v1")
            .WithTags(Tags)
            .WithName(nameof(GetSteamBuildId))
            .WithSummary("Get steam application build id")
            .WithDescription("Get the build ID of the steam version of the game.");

        app.MapGet("v1/application/steam/size", ([FromServices] PalworldSteamApplicationEndpoints endpoints) => endpoints.GetSteamApplicationSize())
            .WithVersion("v1")
            .WithTags(Tags)
            .WithName(nameof(GetSteamApplicationSize))
            .WithSummary("Get steam application size")
            .WithDescription("Get the size of the steam version of the game.");
    }
}
