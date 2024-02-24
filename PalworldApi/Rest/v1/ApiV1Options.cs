using NJsonSchema;
using NSwag.Generation.AspNetCore;
using PalworldApi.Rest.OpenApi.AcceptLanguage;
using PalworldApi.Rest.OpenApi.PalworldVersion;

namespace PalworldApi.Rest.v1;

class ApiV1Options
{
    readonly AspNetCoreOpenApiDocumentGeneratorSettings _options;
    (string Name, JsonSchema JsonSchema)? _languagesSchema;
    (string Name, JsonSchema JsonSchema)? _versionsSchema;

    public ApiV1Options(AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        _options = options;
    }

    public ApiV1Options ConfigureLanguages(IEnumerable<string> languages, string? defaultLanguage = null)
    {
        JsonSchema schema = new();
        schema.Description = "Available languages";
        foreach (string language in languages)
        {
            schema.Enumeration.Add(language);
        }
        schema.Default = defaultLanguage;
        _languagesSchema = ("AcceptLanguage", schema);

        AddAcceptLanguageHeaderOperationProcessor processor = new(schema);
        if (!_options.OperationProcessors.Replace<AddAcceptLanguageHeaderOperationProcessor>(processor))
        {
            _options.OperationProcessors.Add(processor);
        }

        return this;
    }

    public ApiV1Options ConfigureVersions(IEnumerable<string> versions, string? defaultVersion = null)
    {
        JsonSchema schema = new();
        schema.Description = "Available palworld versions";
        foreach (string version in versions)
        {
            schema.Enumeration.Add(version);
        }
        schema.Default = defaultVersion;
        _versionsSchema = ("AcceptPalworldVersion", schema);

        AddPalworldVersionHeaderOperationProcessor processor = new(schema);
        if (!_options.OperationProcessors.Replace<AddPalworldVersionHeaderOperationProcessor>(processor))
        {
            _options.OperationProcessors.Add(processor);
        }

        return this;
    }

    public IEnumerable<(string, JsonSchema)> GetSchemasToAppendToDocument()
    {
        if (_languagesSchema.HasValue)
        {
            yield return _languagesSchema.Value;
        }

        if (_versionsSchema.HasValue)
        {
            yield return _versionsSchema.Value;
        }
    }
}
