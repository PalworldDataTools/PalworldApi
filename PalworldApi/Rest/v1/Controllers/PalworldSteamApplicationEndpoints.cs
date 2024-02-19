using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Services;
using PalworldDataExtractor.Models.Steam;

namespace PalworldApi.Rest.v1.Controllers;

[ApiController]
[Route("v1/application/steam")]
[OpenApiTag("Steam application")]
[OpenApiVersion("v1")]
public class PalworldSteamApplicationEndpoints : ControllerBase
{
    const string Tags = "Steam application";

    readonly RawDataService _rawDataService;

    public PalworldSteamApplicationEndpoints(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    /// <summary>
    ///     Get steam application id
    /// </summary>
    /// <remarks>
    ///     Get the application ID of the steam version of the game.
    /// </remarks>
    /// <returns>The ID of the steam application of the game</returns>
    [HttpGet("id")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<string>, ProblemHttpResult>> GetSteamApplicationId()
    {
        SteamManifest? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return ManifestNotFound(RawDataService.DefaultVersion);
        }

        return TypedResults.Ok(manifest.AppId);
    }

    /// <summary>
    ///     Get steam application name
    /// </summary>
    /// <remarks>
    ///     Get the name of the steam version of the game.
    /// </remarks>
    /// <returns>The name of the steam application of the game</returns>
    [HttpGet("name")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<string>, ProblemHttpResult>> GetSteamApplicationName()
    {
        SteamManifest? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return ManifestNotFound(RawDataService.DefaultVersion);
        }

        return TypedResults.Ok(manifest.AppName);
    }

    /// <summary>
    ///     Get steam application build id
    /// </summary>
    /// <remarks>
    ///     Get the build ID of the steam version of the game.
    /// </remarks>
    /// <returns>The build id of the steam application of the game</returns>
    [HttpGet("build-id")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<string>, ProblemHttpResult>> GetSteamBuildId()
    {
        SteamManifest? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return ManifestNotFound(RawDataService.DefaultVersion);
        }

        return TypedResults.Ok(manifest.BuildId);
    }

    /// <summary>
    ///     Get steam application size
    /// </summary>
    /// <remarks>
    ///     Get the size of the steam version of the game.
    /// </remarks>
    /// <returns>The size of the steam application of the game</returns>
    [HttpGet("size")]
    [ProducesResponseType<long>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<long>, ProblemHttpResult>> GetSteamApplicationSize()
    {
        SteamManifest? manifest = await TryGetManifest();
        if (manifest == null)
        {
            return ManifestNotFound(RawDataService.DefaultVersion);
        }

        return TypedResults.Ok(manifest.AppSize);
    }

    async Task<SteamManifest?> TryGetManifest() => (await _rawDataService.GetData(RawDataService.DefaultVersion))?.SteamManifest;

    static ProblemHttpResult ManifestNotFound(string version) =>
        TypedResults.Problem($"Could not find steam manifest for version: {version}", statusCode: StatusCodes.Status404NotFound);
}
