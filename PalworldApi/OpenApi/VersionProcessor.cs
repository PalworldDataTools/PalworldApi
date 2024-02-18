using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.OpenApi;

public class VersionProcessor : IOperationProcessor
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
        if (versionMetadata == null)
        {
            return false;
        }

        return versionMetadata.Version == ExpectedVersion;
    }
}
