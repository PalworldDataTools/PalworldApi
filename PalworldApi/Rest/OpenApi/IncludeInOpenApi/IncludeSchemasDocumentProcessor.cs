using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.Rest.OpenApi.IncludeInOpenApi;

class IncludeSchemasDocumentProcessor : IDocumentProcessor
{
    readonly (string, JsonSchema)[] _schemas;

    public IncludeSchemasDocumentProcessor(IEnumerable<(string, JsonSchema)> schemas)
    {
        _schemas = schemas.ToArray();
    }

    public void Process(DocumentProcessorContext context)
    {
        foreach ((string? name, JsonSchema? schema) in _schemas)
        {
            context.SchemaResolver.AppendSchema(schema, name);
        }
    }
}
