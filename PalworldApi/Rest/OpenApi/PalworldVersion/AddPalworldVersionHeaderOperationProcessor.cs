using System.Reflection;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.Rest.OpenApi.PalworldVersion;

class AddPalworldVersionHeaderOperationProcessor : IOperationProcessor
{
    readonly JsonSchema _availableVersionsEnumeration;

    public AddPalworldVersionHeaderOperationProcessor(JsonSchema availableVersionsEnumeration)
    {
        _availableVersionsEnumeration = availableVersionsEnumeration;
    }

    public bool Process(OperationProcessorContext context)
    {
        if (context.MethodInfo.GetCustomAttribute<UsePalworldVersionHeaderAttribute>() == null
            && context.MethodInfo.DeclaringType?.GetCustomAttribute<UsePalworldVersionHeaderAttribute>() == null)
        {
            return true;
        }

        OpenApiParameter parameter = new()
        {
            Name = Constants.PalworldVersionRequestHeader,
            Kind = OpenApiParameterKind.Header,
            Schema = new JsonSchema
            {
                Reference = _availableVersionsEnumeration
            },
            IsRequired = false,
            IsNullableRaw = true,
            Description = "The version of Palworld to read data from."
        };

        context.OperationDescription.Operation.Parameters.Add(parameter);

        return true;
    }
}
