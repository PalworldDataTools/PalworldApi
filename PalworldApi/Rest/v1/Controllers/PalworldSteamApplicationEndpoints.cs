using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Services;
using PalworldDataExtractor.Models.Steam;

namespace PalworldApi.Rest.v1.Controllers;

public class PalworldSteamApplicationEndpoints
{
    const string Tags = "Steam application";

    readonly RawDataService _rawDataService;

    public PalworldSteamApplicationEndpoints(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    public async Task<Results<Ok<string>, NotFound>> GetSteamApplicationId()
    {
        SteamManifest? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return ManifestNotFound();
        }

        return TypedResults.Ok(manifest.AppId);
    }

    public async Task<Results<Ok<string>, NotFound>> GetSteamApplicationName()
    {
        SteamManifest? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return ManifestNotFound();
        }

        return TypedResults.Ok(manifest.AppName);
    }

    public async Task<Results<Ok<string>, NotFound>> GetSteamBuildId()
    {
        SteamManifest? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return ManifestNotFound();
        }

        return TypedResults.Ok(manifest.BuildId);
    }

    public async Task<Results<Ok<long>, NotFound>> GetSteamApplicationSize()
    {
        SteamManifest? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return ManifestNotFound();
        }

        return TypedResults.Ok(manifest.AppSize);
    }

    async Task<SteamManifest?> TryGetManifest() => (await _rawDataService.GetData())?.SteamManifest;

    static NotFound ManifestNotFound() => TypedResults.NotFound();

    public static void Map(WebApplication app)
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
