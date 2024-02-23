using System.Reflection;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.Rest.OpenApi.PalworldVersion;

class AddPalworldVersionHeaderOperationProcessor : IOperationProcessor
{
    public AddPalworldVersionHeaderOperationProcessor(IReadOnlyCollection<string> availableVersions, string? defaultVersion = null)
    {
        AvailableVersions = availableVersions;
        DefaultVersion = defaultVersion;
    }

    public IReadOnlyCollection<string> AvailableVersions { get; private set; }
    public string? DefaultVersion { get; private set; }

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
                Type = JsonObjectType.String,
                Item = new JsonSchema { Type = JsonObjectType.String }
            },
            IsRequired = false,
            IsNullableRaw = true,
            Description = "The version of Palworld to read data from.",
            Default = DefaultVersion ?? AvailableVersions.FirstOrDefault()
        };

        foreach (string value in AvailableVersions)
        {
            parameter.Schema.Enumeration.Add(value);
        }

        context.OperationDescription.Operation.Parameters.Add(parameter);

        return true;
    }
}
