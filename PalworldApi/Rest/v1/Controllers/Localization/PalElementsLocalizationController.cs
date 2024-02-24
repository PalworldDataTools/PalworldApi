using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Models;
using PalworldApi.Rest.OpenApi.AcceptLanguage;
using PalworldApi.Rest.OpenApi.OpenApiVersion;
using PalworldApi.Rest.OpenApi.PalworldVersion;
using PalworldApi.Rest.v1.Models.Pals;
using PalworldApi.Services;

namespace PalworldApi.Rest.v1.Controllers.Localization;

/// <summary>
///     Get localized texts for elements
/// </summary>
[ApiController]
[Route("v1/localization/pal-elements")]
[OpenApiTag("Localization")]
[OpenApiVersion("v1")]
public class PalElementsLocalizationController : ControllerBase
{
    readonly RawDataService _rawDataService;
    readonly LocalizationService _localizationService;

    /// <summary>
    ///     Create the pal elements localization controller
    /// </summary>
    public PalElementsLocalizationController(RawDataService rawDataService, LocalizationService localizationService)
    {
        _rawDataService = rawDataService;
        _localizationService = localizationService;
    }

    /// <summary>
    ///     Get pal elements texts
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<Dictionary<PalElement, string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [UseAcceptLanguageHeader]
    [UsePalworldVersionHeader]
    public async Task<Results<Ok<Dictionary<PalElement, string?>>, ProblemHttpResult>> GetPalElementsTexts()
    {
        string palworldVersion = HttpContext.Features.Get<PalworldVersionFeature>()?.Version ?? RawDataService.DefaultVersion;
        VersionedData? data = await _rawDataService.GetData(palworldVersion);
        if (data == null)
        {
            return DataNotFound(palworldVersion);
        }

        string language = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? LocalizationService.DefaultLanguage;
        Localizer? localizer = await _localizationService.GetLocalizer(language);
        if (localizer == null)
        {
            return LanguageNotFound(language);
        }

        return TypedResults.Ok(
            data.Data.Tribes.SelectMany(t => t.Pals)
                .SelectMany(p => new[] { p.ElementType1, p.ElementType2 })
                .Distinct()
                .ToDictionary(e => e.ToPalElement(), e => localizer.Localize($"DT_UI_Common_Text.COMMON_ELEMENT_NAME_{e}"))
        );
    }

    static ProblemHttpResult LanguageNotFound(string language) => TypedResults.Problem($"Could not find language: {language}", statusCode: StatusCodes.Status404NotFound);
    static ProblemHttpResult DataNotFound(string version) => TypedResults.Problem($"Could not find data for version: {version}", statusCode: StatusCodes.Status404NotFound);
}
