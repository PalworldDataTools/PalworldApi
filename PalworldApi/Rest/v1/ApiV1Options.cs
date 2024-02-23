using NSwag.Generation.AspNetCore;
using PalworldApi.Rest.OpenApi.AcceptLanguage;
using PalworldApi.Rest.OpenApi.PalworldVersion;

namespace PalworldApi.Rest.v1;

class ApiV1Options
{
    readonly AspNetCoreOpenApiDocumentGeneratorSettings _options;

    public ApiV1Options(AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        _options = options;
    }

    public ApiV1Options ConfigureLanguages(IEnumerable<string> languages, string? defaultLanguage = null)
    {
        AddAcceptLanguageHeaderOperationProcessor processor = new(languages.ToArray(), defaultLanguage);
        if (!_options.OperationProcessors.Replace<AddAcceptLanguageHeaderOperationProcessor>(processor))
        {
            _options.OperationProcessors.Add(processor);
        }

        return this;
    }

    public ApiV1Options ConfigureVersions(IEnumerable<string> versions, string? defaultVersion = null)
    {
        AddPalworldVersionHeaderOperationProcessor processor = new(versions.ToArray(), defaultVersion);
        if (!_options.OperationProcessors.Replace<AddPalworldVersionHeaderOperationProcessor>(processor))
        {
            _options.OperationProcessors.Add(processor);
        }

        return this;
    }
}
