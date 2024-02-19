using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Services;
using PalworldDataExtractor.Abstractions;
using PalworldDataExtractor.Abstractions.Pals;

namespace PalworldApi.Rest.v1.Controllers;

[ApiController]
[Route("v1/pals")]
[OpenApiTag("Pals")]
[OpenApiVersion("v1")]
public class PalsEndpoints : ControllerBase
{
    readonly RawDataService _rawDataService;

    public PalsEndpoints(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    /// <summary>
    ///     Get Pal
    /// </summary>
    /// <remarks>
    ///     Get the pal with the given name. If multiple variants of the pal are found, the main one is returned. The main variant is the one that is not a boss, nor a gym boss.
    /// </remarks>
    /// <param name="name">The name of the pal to get</param>
    /// <returns>The pal with the given name</returns>
    [HttpGet("{name}")]
    [ProducesResponseType<Pal>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<Pal>, ProblemHttpResult>> GetPal(string name)
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        Pal? pal = data.Tribes.FirstOrDefault(t => t.Name == name)?.Pals.FirstOrDefault();
        if (pal == null)
        {
            return PalNotFound(name);
        }

        return TypedResults.Ok(pal);
    }

    /// <summary>
    ///     Get pal names
    /// </summary>
    /// <remarks>
    ///     "Get the names of all the pals."
    /// </remarks>
    [HttpGet("names")]
    [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<IEnumerable<string>>, ProblemHttpResult>> GetPalNames()
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        return TypedResults.Ok(data.Tribes.Select(t => t.Name));
    }

    static ProblemHttpResult PalNotFound(string name) => TypedResults.Problem($"Could not find pal with name: {name}", statusCode: StatusCodes.Status404NotFound);
    static ProblemHttpResult DataNotFound(string version) => TypedResults.Problem($"Could not find data for version: {version}", statusCode: StatusCodes.Status404NotFound);
}
