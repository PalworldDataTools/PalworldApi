using System.Reflection;
using PalworldDataExtractor.Abstractions;

namespace PalworldApi.Services;

public class RawDataService
{
    static readonly Dictionary<string, string> Versions = new()
    {
        { "steam-13390747", "steam-13390747.json" }
    };
    internal const string DefaultVersion = "steam-13390747";

    readonly ILogger<RawDataService> _logger;
    readonly Dictionary<string, ExtractedData> _cachedData = new();

    public RawDataService(ILogger<RawDataService> logger)
    {
        _logger = logger;
    }

    public IReadOnlyCollection<string> GetVersions() => Versions.Keys;

    public async Task<ExtractedData?> GetData(string version)
    {
        if (_cachedData.TryGetValue(version, out ExtractedData? cachedData))
        {
            return cachedData;
        }

        return await LoadAndCache(version);
    }

    async Task<ExtractedData?> LoadAndCache(string version)
    {
        if (!Versions.TryGetValue(version, out string? resourcePath))
        {
            return null;
        }

        Stream? resource = Assembly.GetExecutingAssembly().GetManifestResourceStream($"PalworldApi.Resources.PalworldData.{resourcePath}");
        if (resource == null)
        {
            return null;
        }

        ExtractedData? data = await ExtractedData.Deserialize(resource);
        if (data == null)
        {
            return null;
        }

        _logger.LogInformation("Palworld data has been loaded: {version}", version);

        _cachedData[version] = data;

        return data;
    }
}
