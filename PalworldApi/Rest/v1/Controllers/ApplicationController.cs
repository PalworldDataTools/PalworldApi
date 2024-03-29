﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PalworldApi.Rest.OpenApi.OpenApiVersion;
using PalworldApi.Rest.OpenApi.PalworldVersion;
using PalworldApi.Services;

namespace PalworldApi.Rest.v1.Controllers;

/// <summary>
///     Get data about the application itself
/// </summary>
[ApiController]
[Route("v1/application")]
[OpenApiTag("Application")]
[OpenApiVersion("v1")]
public class ApplicationController : ControllerBase
{
    readonly RawDataService _rawDataService;
    readonly LocalizationService _localizationService;

    /// <summary>
    ///     Create the pals data controller
    /// </summary>
    public ApplicationController(RawDataService rawDataService, LocalizationService localizationService)
    {
        _rawDataService = rawDataService;
        _localizationService = localizationService;
    }

    /// <summary>
    ///     Get available versions
    /// </summary>
    /// <remarks>
    ///     Get all the available versions. The APIs can use different versions of the game to read their data from.
    /// </remarks>
    [HttpGet("palworld-versions")]
    [ProducesResponseType<IReadOnlyCollection<string>>(StatusCodes.Status200OK)]
    public Ok<IReadOnlyCollection<string>> GetPalworldVersions() => TypedResults.Ok(_rawDataService.GetVersions());

    /// <summary>
    ///     Get available languages
    /// </summary>
    /// <remarks>
    ///     Get all the available languages. The APIs can use different languages to localize the data before returning their responses.
    /// </remarks>
    [HttpGet("languages")]
    [ProducesResponseType<IReadOnlyCollection<string>>(StatusCodes.Status200OK)]
    [UsePalworldVersionHeader]
    public async Task<Ok<IReadOnlyCollection<string>>> GetLanguages()
    {
        string palworldVersion = HttpContext.Features.Get<PalworldVersionFeature>()?.Version ?? RawDataService.DefaultVersion;
        return TypedResults.Ok(await _localizationService.GetLanguages(palworldVersion));
    }
}
