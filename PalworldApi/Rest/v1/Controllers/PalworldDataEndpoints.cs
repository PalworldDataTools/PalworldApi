using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Services;

namespace PalworldApi.Rest.v1.Controllers;

public class PalworldDataEndpoints
{
    const string Tags = "Palworld";
    readonly RawDataService _rawDataService;

    public PalworldDataEndpoints(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    public Ok<IReadOnlyCollection<string>> GetPalworldVersions() => TypedResults.Ok(_rawDataService.GetVersions());

    public static void Map(WebApplication app) =>
        app.MapGet("v1/palworld/versions", ([FromServices] PalworldDataEndpoints endpoints) => endpoints.GetPalworldVersions())
            .WithVersion("v1")
            .WithTags(Tags)
            .WithName(nameof(GetPalworldVersions))
            .WithSummary("Get available versions")
            .WithDescription("Get all the available versions. The APIs can use different versions of the game to read their data from.");
}
