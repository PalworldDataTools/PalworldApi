namespace PalworldApi.Rest.OpenApi.PalworldVersion;

class PalworldVersionMiddleware
{
    readonly RequestDelegate _next;
    readonly ILogger<PalworldVersionMiddleware> _logger;

    public PalworldVersionMiddleware(RequestDelegate next, ILogger<PalworldVersionMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext context)
    {
        string? version = context.Request.Headers[Constants.PalworldVersionRequestHeader].FirstOrDefault();

        if (!string.IsNullOrEmpty(version))
        {
            context.Features.Set(new PalworldVersionFeature(version));
            context.Response.Headers.Append(Constants.PalworldVersionResponseHeader, version);
        }

        await _next(context);
    }
}
