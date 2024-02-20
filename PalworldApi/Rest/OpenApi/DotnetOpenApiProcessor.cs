using Microsoft.OpenApi.Models;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace PalworldApi.Rest.OpenApi;

class DotnetOpenApiProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        if (context is not AspNetCoreOperationProcessorContext aspnetContext)
        {
            // Will still include this operation - set to false to exclude it.
            return true;
        }

        foreach (object metadata in aspnetContext.ApiDescription.ActionDescriptor.EndpointMetadata)
        {

            if (metadata is OpenApiOperation openApiMetadata)
            {
                context.OperationDescription.Operation.OperationId = openApiMetadata.OperationId;
                context.OperationDescription.Operation.IsDeprecated = openApiMetadata.Deprecated;
                context.OperationDescription.Operation.Summary = openApiMetadata.Summary;
                context.OperationDescription.Operation.Description = openApiMetadata.Description;
            }
            else if (metadata is EndpointSummaryAttribute summaryAttribute)
            {
                context.OperationDescription.Operation.Summary = summaryAttribute.Summary;
            }
            else if (metadata is EndpointDescriptionAttribute descriptionAttribute)
            {
                context.OperationDescription.Operation.Description = descriptionAttribute.Description;
            }
            else if (metadata is EndpointNameAttribute nameAttribute)
            {
                context.OperationDescription.Operation.OperationId = nameAttribute.EndpointName;
            }
        }

        return true;
    }
}
