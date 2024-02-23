using System.Reflection;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.Rest.OpenApi.AcceptLanguage;

class AddAcceptLanguageHeaderOperationProcessor : IOperationProcessor
{
    public AddAcceptLanguageHeaderOperationProcessor(IReadOnlyCollection<string> availableLanguages, string? defaultLanguage = null)
    {
        AvailableLanguages = availableLanguages;
        DefaultLanguage = defaultLanguage;
    }

    public IReadOnlyCollection<string> AvailableLanguages { get; private set; }
    public string? DefaultLanguage { get; private set; }

    public bool Process(OperationProcessorContext context)
    {
        if (context.MethodInfo.GetCustomAttribute<UseAcceptLanguageHeaderAttribute>() == null
            && context.MethodInfo.DeclaringType?.GetCustomAttribute<UseAcceptLanguageHeaderAttribute>() == null)
        {
            return true;
        }

        OpenApiParameter parameter = new()
        {
            Name = "Accept-Language",
            Kind = OpenApiParameterKind.Header,
            Schema = new JsonSchema
            {
                Type = JsonObjectType.String,
                Item = new JsonSchema { Type = JsonObjectType.String }
            },
            IsRequired = false,
            IsNullableRaw = true,
            Description = "The Accept-Language request HTTP header indicates the natural language and locale that the client prefers. "
                          + "See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language.",
            Default = DefaultLanguage ?? AvailableLanguages.FirstOrDefault()
        };

        foreach (string value in AvailableLanguages)
        {
            parameter.Schema.Enumeration.Add(value);
        }

        context.OperationDescription.Operation.Parameters.Add(parameter);

        return true;
    }
}
