﻿using PalworldApi.Rest.OpenApi;
using PalworldApi.Rest.OpenApi.IncludeInOpenApi;
using PalworldApi.Rest.OpenApi.OpenApiVersion;

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

                ApiV1Options apiV1Options = new(opt);
                options?.Invoke(apiV1Options);

                opt.DocumentProcessors.Add(new IncludeSchemasDocumentProcessor(apiV1Options.GetSchemasToAppendToDocument()));
            }
        );
}
