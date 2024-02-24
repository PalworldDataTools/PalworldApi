using System.Reflection;
using Namotion.Reflection;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.Rest.OpenApi.IncludeInOpenApi;

class IncludeAdditionalModelsDocumentProcessor : IDocumentProcessor
{
    public void Process(DocumentProcessorContext context)
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute<IncludeInOpenApiAttribute>() != null))
        {
            context.SchemaGenerator.Generate(type.ToContextualType(), context.SchemaResolver);
        }
    }
}
