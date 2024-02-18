namespace PalworldApi.Rest.OpenApi;

public class VersionMetadata
{
    public string Version { get; }

    public VersionMetadata(string version)
    {
        Version = version;
    }
}

public static class VersionMetadataExtensions
{
    public static TBuilder WithVersion<TBuilder>(this TBuilder builder, string version) where TBuilder: IEndpointConventionBuilder =>
        builder.WithMetadata(new VersionMetadata(version));
}
