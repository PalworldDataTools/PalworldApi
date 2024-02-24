namespace PalworldApi.Rest.OpenApi.OpenApiVersion;

class VersionMetadata
{
    public string Version { get; }

    public VersionMetadata(string version)
    {
        Version = version;
    }
}

static class VersionMetadataExtensions
{
    public static TBuilder WithVersion<TBuilder>(this TBuilder builder, string version) where TBuilder: IEndpointConventionBuilder =>
        builder.WithMetadata(new VersionMetadata(version));
}
