using System.Reflection;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.Rest.OpenApi.OpenApiVersion;

class VersionProcessor : IOperationProcessor
{
    public VersionProcessor(string expectedVersion)
    {
        ExpectedVersion = expectedVersion;
    }

    public string ExpectedVersion { get; private set; }

    public bool Process(OperationProcessorContext context)
    {
        if (context is not AspNetCoreOperationProcessorContext aspNetCoreOperationProcessorContext)
        {
            return false;
        }

        VersionMetadata? versionMetadata = aspNetCoreOperationProcessorContext.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<VersionMetadata>().FirstOrDefault();
        if (versionMetadata != null)
        {
            return versionMetadata.Version == ExpectedVersion;
        }

        OpenApiVersionAttribute? versionAttribute = aspNetCoreOperationProcessorContext.MethodInfo.GetCustomAttribute<OpenApiVersionAttribute>()
                                                    ?? aspNetCoreOperationProcessorContext.MethodInfo.DeclaringType?.GetCustomAttribute<OpenApiVersionAttribute>();
        if (versionAttribute != null)
        {
            return versionAttribute.Version == ExpectedVersion;
        }

        return false;
    }
}
