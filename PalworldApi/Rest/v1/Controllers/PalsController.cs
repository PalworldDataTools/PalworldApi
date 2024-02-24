using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Models;
using PalworldApi.Models.Search;
using PalworldApi.Requests.SearchPalTribes;
using PalworldApi.Rest.OpenApi.AcceptLanguage;
using PalworldApi.Rest.OpenApi.OpenApiVersion;
using PalworldApi.Rest.OpenApi.PalworldVersion;
using PalworldApi.Rest.v1.Models.PalIcons;
using PalworldApi.Rest.v1.Models.Pals;
using PalworldApi.Services;
using Pal = PalworldApi.Rest.v1.Models.Pals.Pal;
using PalTribe = PalworldApi.Rest.v1.Models.Pals.PalTribe;

namespace PalworldApi.Rest.v1.Controllers;

/// <summary>
///     Get data about pals
/// </summary>
[ApiController]
[Route("v1/pals")]
[OpenApiTag("Pals")]
[OpenApiVersion("v1")]
public class PalsController : ControllerBase
{
    readonly RawDataService _rawDataService;
    readonly PalIconsService _palIconsService;
    readonly LocalizationService _localizationService;
    readonly IMediator _mediator;

    /// <summary>
    ///     Create the pals controller
    /// </summary>
    public PalsController(RawDataService rawDataService, PalIconsService palIconsService, LocalizationService localizationService, IMediator mediator)
    {
        _rawDataService = rawDataService;
        _mediator = mediator;
        _localizationService = localizationService;
        _palIconsService = palIconsService;
    }

    /// <summary>
    ///     Search tribes
    /// </summary>
    /// <remarks>
    ///     Get the names of all the tribes.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType<SearchResult<PalTribe>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [UseAcceptLanguageHeader]
    [UsePalworldVersionHeader]
    public async Task<Results<Ok<SearchResult<PalTribe>>, ProblemHttpResult>> SearchTribes([FromQuery] SearchRequest<PalsFilters> request)
    {
        string palworldVersion = HttpContext.Features.Get<PalworldVersionFeature>()?.Version ?? RawDataService.DefaultVersion;
        VersionedData? data = await _rawDataService.GetData(palworldVersion);
        if (data == null)
        {
            return DataNotFound(palworldVersion);
        }

        SearchResult<PalworldDataExtractor.Abstractions.Pals.PalTribe> searchResult = await _mediator.Send(new SearchPalTribesRequest { Data = data, SearchRequest = request });

        string language = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? LocalizationService.DefaultLanguage;
        Localizer? localizer = await _localizationService.GetLocalizer(language);

        return TypedResults.Ok(searchResult.Select(tribe => tribe.ToV1(localizer)));
    }

    /// <summary>
    ///     Get tribe
    /// </summary>
    /// <remarks>
    ///     Get the tribe of pals with the given name.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The tribe of pals with the given name</returns>
    [HttpGet("{tribeName}")]
    [ProducesResponseType<PalTribe>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [UseAcceptLanguageHeader]
    [UsePalworldVersionHeader]
    public async Task<Results<Ok<PalTribe>, ProblemHttpResult>> GetTribe(string tribeName)
    {
        string palworldVersion = HttpContext.Features.Get<PalworldVersionFeature>()?.Version ?? RawDataService.DefaultVersion;
        VersionedData? data = await _rawDataService.GetData(palworldVersion);
        if (data == null)
        {
            return DataNotFound(palworldVersion);
        }

        PalworldDataExtractor.Abstractions.Pals.PalTribe? tribe = data.Data.Tribes.FirstOrDefault(t => t.Name == tribeName);
        if (tribe == null)
        {
            return TribeNotFound(tribeName);
        }

        string language = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? LocalizationService.DefaultLanguage;
        Localizer? localizer = await _localizationService.GetLocalizer(language);

        return TypedResults.Ok(tribe.ToV1(localizer));
    }

    /// <summary>
    ///     Get tribe icon
    /// </summary>
    /// <remarks>
    ///     Get the icon of the given tribe.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <param name="size">The size of the returned icon</param>
    /// <returns>The icon of the given tribe</returns>
    [HttpGet("{tribeName}/icon")]
    [ProducesResponseType<ActionResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [UsePalworldVersionHeader]
    public async Task<Results<FileContentHttpResult, ProblemHttpResult>> GetIcon(string tribeName, PalIconSize size)
    {
        string palworldVersion = HttpContext.Features.Get<PalworldVersionFeature>()?.Version ?? RawDataService.DefaultVersion;
        (int, int)? sizePx = size switch
        {
            PalIconSize.Original => null,
            PalIconSize.Small => (64, 64),
            PalIconSize.Medium => (256, 256),
            PalIconSize.Big => (512, 512),
            _ => null
        };

        byte[]? icon = await _palIconsService.GetPalIconAsync(tribeName, sizePx, palworldVersion);
        if (icon == null)
        {
            return IconNotFound(tribeName, palworldVersion);
        }

        return TypedResults.File(icon, "image/png", tribeName + ".png");
    }

    /// <summary>
    ///     Get pal
    /// </summary>
    /// <remarks>
    ///     Get the main pal of the given tribe. The main pal is the one that is neither a boss, nor a gym boss.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The main pal of the requested tribe</returns>
    [HttpGet("{tribeName}/main")]
    [ProducesResponseType<Pal>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [UseAcceptLanguageHeader]
    [UsePalworldVersionHeader]
    public async Task<Results<Ok<Pal>, ProblemHttpResult>> GetPal(string tribeName)
    {
        string palworldVersion = HttpContext.Features.Get<PalworldVersionFeature>()?.Version ?? RawDataService.DefaultVersion;
        VersionedData? data = await _rawDataService.GetData(palworldVersion);
        if (data == null)
        {
            return DataNotFound(palworldVersion);
        }

        PalworldDataExtractor.Abstractions.Pals.PalTribe? tribe = data.Data.Tribes.FirstOrDefault(t => t.Name == tribeName);
        if (tribe == null)
        {
            return TribeNotFound(tribeName);
        }

        PalworldDataExtractor.Abstractions.Pals.Pal mainPal = tribe.GetMainPal();

        string language = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? LocalizationService.DefaultLanguage;
        Localizer? localizer = await _localizationService.GetLocalizer(language);

        return TypedResults.Ok(mainPal.ToV1(localizer));
    }

    /// <summary>
    ///     Get BOSS pal
    /// </summary>
    /// <remarks>
    ///     Get the boss pal of the given tribe.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The boss pal of the requested tribe</returns>
    [HttpGet("{tribeName}/boss")]
    [ProducesResponseType<Pal>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [UseAcceptLanguageHeader]
    [UsePalworldVersionHeader]
    public async Task<Results<Ok<Pal>, ProblemHttpResult>> GetBossPal(string tribeName)
    {
        string palworldVersion = HttpContext.Features.Get<PalworldVersionFeature>()?.Version ?? RawDataService.DefaultVersion;
        VersionedData? data = await _rawDataService.GetData(palworldVersion);
        if (data == null)
        {
            return DataNotFound(palworldVersion);
        }

        PalworldDataExtractor.Abstractions.Pals.PalTribe? tribe = data.Data.Tribes.FirstOrDefault(t => t.Name == tribeName);
        if (tribe == null)
        {
            return TribeNotFound(tribeName);
        }

        PalworldDataExtractor.Abstractions.Pals.Pal? mainPal = tribe.GetBossPal();
        if (mainPal == null)
        {
            return PalNotFound(tribeName, "boss");
        }

        string language = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? LocalizationService.DefaultLanguage;
        Localizer? localizer = await _localizationService.GetLocalizer(language);

        return TypedResults.Ok(mainPal.ToV1(localizer));
    }

    /// <summary>
    ///     Get GYM pal
    /// </summary>
    /// <remarks>
    ///     Get the gym pal of the given tribe.
    /// </remarks>
    /// <param name="tribeName">The name of the tribe to get</param>
    /// <returns>The gym pal of the requested tribe</returns>
    [HttpGet("{tribeName}/gym")]
    [ProducesResponseType<Pal>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [UseAcceptLanguageHeader]
    [UsePalworldVersionHeader]
    public async Task<Results<Ok<Pal>, ProblemHttpResult>> GetGymPal(string tribeName)
    {
        string palworldVersion = HttpContext.Features.Get<PalworldVersionFeature>()?.Version ?? RawDataService.DefaultVersion;
        VersionedData? data = await _rawDataService.GetData(palworldVersion);
        if (data == null)
        {
            return DataNotFound(palworldVersion);
        }

        PalworldDataExtractor.Abstractions.Pals.PalTribe? tribe = data.Data.Tribes.FirstOrDefault(t => t.Name == tribeName);
        if (tribe == null)
        {
            return TribeNotFound(tribeName);
        }

        PalworldDataExtractor.Abstractions.Pals.Pal? mainPal = tribe.GetGymPal();
        if (mainPal == null)
        {
            return PalNotFound(tribeName, "gym");
        }

        string language = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? LocalizationService.DefaultLanguage;
        Localizer? localizer = await _localizationService.GetLocalizer(language);

        return TypedResults.Ok(mainPal.ToV1(localizer));
    }

    static ProblemHttpResult IconNotFound(string name, string version) =>
        TypedResults.Problem($"Could not find icon for tribe with name {name} in version {version}", statusCode: StatusCodes.Status404NotFound);

    static ProblemHttpResult PalNotFound(string name, string variant) =>
        TypedResults.Problem($"Could not find {variant} pal of tribe: {name}", statusCode: StatusCodes.Status404NotFound);

    static ProblemHttpResult TribeNotFound(string name) => TypedResults.Problem($"Could not find tribe with name: {name}", statusCode: StatusCodes.Status404NotFound);
    static ProblemHttpResult DataNotFound(string version) => TypedResults.Problem($"Could not find data for version: {version}", statusCode: StatusCodes.Status404NotFound);
}
