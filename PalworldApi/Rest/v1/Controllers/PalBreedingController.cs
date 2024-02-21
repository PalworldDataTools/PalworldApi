using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Requests.Breeding;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Rest.v1.Models.Pals;
using PalworldApi.Services;
using PalworldDataExtractor.Abstractions;
using PalTribe = PalworldDataExtractor.Abstractions.Pals.PalTribe;

namespace PalworldApi.Rest.v1.Controllers;

/// <summary>
///     Get data about breeding
/// </summary>
[ApiController]
[Route("v1/pals/breed")]
[OpenApiTag("Breeding")]
[OpenApiVersion("v1")]
[ResponseCache(CacheProfileName = Constants.ResponseCacheDefaultProfile)]
public class PalBreedingController : ControllerBase
{
    readonly RawDataService _rawDataService;
    readonly IMediator _mediator;

    /// <summary>
    ///     Create the pal breeding controller
    /// </summary>
    public PalBreedingController(RawDataService rawDataService, IMediator mediator)
    {
        _rawDataService = rawDataService;
        _mediator = mediator;
    }

    /// <summary>
    ///     Get breeding result
    /// </summary>
    /// <param name="palNameA">The name of the first pal</param>
    /// <param name="palNameB">The name of the second pal</param>
    /// <returns>The name of the tribe that is the result of the breeding</returns>
    [HttpGet]
    [ProducesResponseType<PalTribe>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemHttpResult>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<Pal>, ProblemHttpResult>> GetBreedingResult(string palNameA, string palNameB)
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        if (!TryFindPal(data, palNameA, out PalworldDataExtractor.Abstractions.Pals.Pal? palA))
        {
            return PalNotFound(palNameA);
        }

        if (!TryFindPal(data, palNameB, out PalworldDataExtractor.Abstractions.Pals.Pal? palB))
        {
            return PalNotFound(palNameB);
        }

        PalworldDataExtractor.Abstractions.Pals.Pal child = await _mediator.Send(new BreedPalsRequest { Data = data, PalA = palA, PalB = palB });

        return TypedResults.Ok(child.ToV1());
    }

    static bool TryFindPal(ExtractedData data, string palName, [NotNullWhen(true)] out PalworldDataExtractor.Abstractions.Pals.Pal? pal)
    {
        pal = data.Tribes.SelectMany(t => t.Pals).FirstOrDefault(t => t.Name == palName);
        return pal != null;
    }

    static ProblemHttpResult PalNotFound(string name, string? message = null) =>
        TypedResults.Problem($"{message ?? "Could not find pal with name"}: {name}", statusCode: StatusCodes.Status404NotFound);

    static ProblemHttpResult DataNotFound(string version) => TypedResults.Problem($"Could not find data for version: {version}", statusCode: StatusCodes.Status404NotFound);
}
