using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Rest.v1.Models.Pals;
using PalworldApi.Services;
using PalworldDataExtractor.Abstractions;
using Pal = PalworldApi.Rest.v1.Models.Pals.Pal;
using PalTribe = PalworldApi.Rest.v1.Models.Pals.PalTribe;

namespace PalworldApi.Rest.v1.Controllers;

[ApiController]
[Route("v1/pals")]
[OpenApiTag("Pals")]
[OpenApiVersion("v1")]
public class PalsController : ControllerBase
{
    readonly RawDataService _rawDataService;

    public PalsController(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    /// <summary>
    ///     Get tribe names
    /// </summary>
    /// <remarks>
    ///     Get the names of all the tribes.
    /// </remarks>
    [HttpGet("names")]
    [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<IEnumerable<string>>, ProblemHttpResult>> GetTribeNames()
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        return TypedResults.Ok(data.Tribes.Select(t => t.Name));
    }

    /// <summary>
    ///     Get Tribe
    /// </summary>
    /// <remarks>
    ///     Get the tribe of pals with the given name.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The tribe of pals with the given name</returns>
    [HttpGet("{tribeName}")]
    [ProducesResponseType<PalTribe>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<PalTribe>, ProblemHttpResult>> GetTribe(string tribeName)
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        PalworldDataExtractor.Abstractions.Pals.PalTribe? tribe = data.Tribes.FirstOrDefault(t => t.Name == tribeName);
        if (tribe == null)
        {
            return TribeNotFound(tribeName);
        }

        return TypedResults.Ok(tribe.ToV1());
    }

    /// <summary>
    ///     Get Pal Icon
    /// </summary>
    /// <remarks>
    ///     Get the icon of the given tribe.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The icon of the given tribe</returns>
    [HttpGet("{tribeName}/icon")]
    [ProducesResponseType<ActionResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<FileContentHttpResult, ProblemHttpResult>> GetIcon(string tribeName)
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        if (!data.TribeIcons.TryGetValue(tribeName, out byte[]? icon))
        {
            return TribeNotFound(tribeName);
        }

        return TypedResults.File(icon, "image/png", tribeName + ".png");
    }

    /// <summary>
    ///     Get Pal
    /// </summary>
    /// <remarks>
    ///     Get the main pal of the given tribe. The main pal is the one that is neither a boss, nor a gym boss.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The main pal of the requested tribe</returns>
    [HttpGet("{tribeName}/main")]
    [ProducesResponseType<Pal>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<Pal>, ProblemHttpResult>> GetPal(string tribeName)
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        PalworldDataExtractor.Abstractions.Pals.PalTribe? tribe = data.Tribes.FirstOrDefault(t => t.Name == tribeName);
        if (tribe == null)
        {
            return TribeNotFound(tribeName);
        }

        PalworldDataExtractor.Abstractions.Pals.Pal? mainPal = tribe.Pals.FirstOrDefault(p => p is { IsBoss: false, IsTowerBoss: false }) ?? tribe.Pals.FirstOrDefault();
        if (mainPal == null)
        {
            return PalNotFound(tribeName, "main");
        }

        return TypedResults.Ok(mainPal.ToV1());
    }

    /// <summary>
    ///     Get BOSS Pal
    /// </summary>
    /// <remarks>
    ///     Get the boss pal of the given tribe.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The boss pal of the requested tribe</returns>
    [HttpGet("{tribeName}/boss")]
    [ProducesResponseType<Pal>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<Pal>, ProblemHttpResult>> GetBossPal(string tribeName)
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        PalworldDataExtractor.Abstractions.Pals.PalTribe? tribe = data.Tribes.FirstOrDefault(t => t.Name == tribeName);
        if (tribe == null)
        {
            return TribeNotFound(tribeName);
        }

        PalworldDataExtractor.Abstractions.Pals.Pal? mainPal = tribe.Pals.FirstOrDefault(p => p is { IsBoss: true, IsTowerBoss: false });
        if (mainPal == null)
        {
            return PalNotFound(tribeName, "boss");
        }

        return TypedResults.Ok(mainPal.ToV1());
    }

    /// <summary>
    ///     Get GYM Pal
    /// </summary>
    /// <remarks>
    ///     Get the gym pal of the given tribe.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The gym pal of the requested tribe</returns>
    [HttpGet("{tribeName}/gym")]
    [ProducesResponseType<Pal>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<Pal>, ProblemHttpResult>> GetGymPal(string tribeName)
    {
        ExtractedData? data = await _rawDataService.GetData(RawDataService.DefaultVersion);
        if (data == null)
        {
            return DataNotFound(RawDataService.DefaultVersion);
        }

        PalworldDataExtractor.Abstractions.Pals.PalTribe? tribe = data.Tribes.FirstOrDefault(t => t.Name == tribeName);
        if (tribe == null)
        {
            return TribeNotFound(tribeName);
        }

        PalworldDataExtractor.Abstractions.Pals.Pal? mainPal = tribe.Pals.FirstOrDefault(p => p is { IsTowerBoss: true });
        if (mainPal == null)
        {
            return PalNotFound(tribeName, "gym");
        }

        return TypedResults.Ok(mainPal.ToV1());
    }

    static ProblemHttpResult PalNotFound(string name, string variant) =>
        TypedResults.Problem($"Could not find {variant} pal of tribe: {name}", statusCode: StatusCodes.Status404NotFound);

    static ProblemHttpResult TribeNotFound(string name) => TypedResults.Problem($"Could not find tribe with name: {name}", statusCode: StatusCodes.Status404NotFound);
    static ProblemHttpResult DataNotFound(string version) => TypedResults.Problem($"Could not find data for version: {version}", statusCode: StatusCodes.Status404NotFound);
}
