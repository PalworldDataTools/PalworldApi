using PalworldApi.Rest.OpenApi;

namespace PalworldApi.Rest.v1;

static class ApiV1Extensions
{
    public static void AddV1(this IServiceCollection services, Action<ApiV1Options>? options = null) =>
        services.AddOpenApiDocument(
            opt =>
            {
                opt.DocumentName = "v1";
                opt.Title = "Palworld API";
                opt.Description = "Browse data from Palworld";
                opt.Version = "1.0.0";
                opt.OperationProcessors.Add(new VersionProcessor("v1"));
                opt.OperationProcessors.Add(new DotnetOpenApiProcessor());
                opt.DocumentProcessors.Add(new IncludeAdditionalModelsDocumentProcessor());

                options?.Invoke(new ApiV1Options(opt));
            }
        );
}
