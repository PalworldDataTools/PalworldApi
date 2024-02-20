using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Services;

namespace PalworldApi.Rest.v1.Controllers;

/// <summary>
///     Get data about the Palworld API application itself
/// </summary>
[ApiController]
[Route("v1/palworld")]
[OpenApiTag("Palworld")]
[OpenApiVersion("v1")]
[ResponseCache(CacheProfileName = Constants.ResponseCacheDefaultProfile)]
public class PalworldApiController : ControllerBase
{
    readonly RawDataService _rawDataService;

    /// <summary>
    ///     Create the pals data controller
    /// </summary>
    public PalworldApiController(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    /// <summary>
    ///     Get available versions
    /// </summary>
    /// <remarks>
    ///     Get all the available versions. The APIs can use different versions of the game to read their data from.
    /// </remarks>
    [HttpGet("versions")]
    [ProducesResponseType<IReadOnlyCollection<string>>(StatusCodes.Status200OK)]
    public Ok<IReadOnlyCollection<string>> GetPalworldVersions() => TypedResults.Ok(_rawDataService.GetVersions());
}
