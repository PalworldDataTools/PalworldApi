using PalworldApi.OpenApi;
using PalworldApi.v1.Controllers;

namespace PalworldApi.v1;

public static class ApiV1Extensions
{
    public static void AddV1(this IServiceCollection services)
    {
        services.AddSingleton<PalworldSteamApplicationEndpoints>();
        services.AddSingleton<PalworldDataEndpoints>();
        services.AddSingleton<PalsEndpoints>();

        services.AddOpenApiDocument(
            opt =>
            {
                opt.DocumentName = "v1";
                opt.Title = "Palworld API";
                opt.Description = "Browse data from Palworld";
                opt.Version = "1.0.0";
                opt.OperationProcessors.Add(new VersionProcessor("v1"));
                opt.OperationProcessors.Add(new DotnetOpenApiProcessor());
            }
        );
    }

    public static void UseV1(this WebApplication app)
    {
        PalworldSteamApplicationEndpoints.Map(app);
        PalworldDataEndpoints.Map(app);
        PalsEndpoints.Map(app);
    }
}
