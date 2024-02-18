using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PalworldApi.OpenApi;
using PalworldApi.Services;
using PalworldDataExtractor.Models.Pals;

namespace PalworldApi.v1.Controllers;

public class PalsEndpoints
{
    const string Tags = "Pals";

    readonly RawDataService _rawDataService;

    public PalsEndpoints(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    public Results<Ok<Pal>, NotFound> GetPal(string name)
    {
        Pal? pal = _rawDataService.Data.Tribes.FirstOrDefault(t => t.Name == name)?.Pals.FirstOrDefault();
        if (pal == null)
        {
            return PalNotFound();
        }

        return TypedResults.Ok(pal);
    }

    public Ok<IEnumerable<string>> GetPalNames() => TypedResults.Ok(_rawDataService.Data.Tribes.Select(t => t.Name));

    static NotFound PalNotFound() => TypedResults.NotFound();

    public static void Map(WebApplication app, string? routePrefix = null)
    {
        app.MapGet("v1/pals/names", ([FromServices] PalsEndpoints endpoints) => endpoints.GetPalNames())
            .WithVersion("v1")
            .WithTags(Tags)
            .WithName(nameof(GetPalNames))
            .WithSummary("Get pal names")
            .WithDescription("Get the names of all the pals.");

        app.MapGet("v1/pals/{name}", ([FromServices] PalsEndpoints endpoints, string name) => endpoints.GetPal(name))
            .WithVersion("v1")
            .WithTags(Tags)
            .WithName(nameof(GetPal))
            .WithSummary("Get pal")
            .WithDescription(
                "Get the pal with the given name. If multiple variants of the pal are found, the main one is returned. The main variant is the one that is not a boss, nor a gym boss."
            );
    }
}
