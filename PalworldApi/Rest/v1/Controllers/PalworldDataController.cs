using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Services;

namespace PalworldApi.Rest.v1.Controllers;

[ApiController]
[Route("v1/palworld")]
[OpenApiTag("Palworld")]
[OpenApiVersion("v1")]
[ResponseCache(CacheProfileName = Constants.ResponseCacheDefaultProfile)]
public class PalworldDataController : ControllerBase
{
    readonly RawDataService _rawDataService;

    public PalworldDataController(RawDataService rawDataService)
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
