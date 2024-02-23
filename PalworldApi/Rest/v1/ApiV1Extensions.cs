using PalworldApi.Rest.OpenApi;
using PalworldApi.Rest.OpenApi.AcceptLanguage;

namespace PalworldApi.Rest.v1;

static class ApiV1Extensions
{
    public static void AddV1(this IServiceCollection services, string[] languages, string defaultLanguage) =>
        services.AddOpenApiDocument(
            opt =>
            {
                opt.DocumentName = "v1";
                opt.Title = "Palworld API";
                opt.Description = "Browse data from Palworld";
                opt.Version = "1.0.0";
                opt.OperationProcessors.Add(new VersionProcessor("v1"));
                opt.OperationProcessors.Add(new DotnetOpenApiProcessor());
                opt.OperationProcessors.Add(new AddAcceptLanguageHeaderOperationProcessor("Accept-Language", languages, defaultLanguage));
                opt.DocumentProcessors.Add(new IncludeAdditionalModelsDocumentProcessor());
            }
        );
}
