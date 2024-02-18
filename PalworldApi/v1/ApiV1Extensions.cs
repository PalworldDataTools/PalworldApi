using PalworldApi.OpenApi;
using PalworldApi.v1.Controllers;

namespace PalworldApi.v1;

public static class ApiV1Extensions
{
    public static void AddV1(this IServiceCollection services)
    {
        services.AddSingleton<PalsEndpoints>();
        services.AddSingleton<PalworldSteamApplicationEndpoints>();

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

    public static void UseV1(this WebApplication app, string? prefix = null)
    {
        prefix = prefix == null
            ? "v1/"
            : prefix.EndsWith('/')
                ? prefix
                : prefix + '/';

        PalworldSteamApplicationEndpoints.Map(app, prefix);
        PalsEndpoints.Map(app, prefix);
    }
}
