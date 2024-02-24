using System.Reflection;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.Rest.OpenApi.AcceptLanguage;

class AddAcceptLanguageHeaderOperationProcessor : IOperationProcessor
{
    readonly JsonSchema _availableVersionsEnumeration;

    public AddAcceptLanguageHeaderOperationProcessor(JsonSchema availableVersionsEnumeration)
    {
        _availableVersionsEnumeration = availableVersionsEnumeration;
    }

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
                Reference = _availableVersionsEnumeration
            },
            IsRequired = false,
            IsNullableRaw = true,
            Description = "The Accept-Language request HTTP header indicates the natural language and locale that the client prefers. "
                          + "See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language."
        };

        context.OperationDescription.Operation.Parameters.Add(parameter);

        return true;
    }
}
