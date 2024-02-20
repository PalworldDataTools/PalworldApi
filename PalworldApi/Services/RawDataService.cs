using System.Reflection;
using PalworldDataExtractor.Abstractions;

namespace PalworldApi.Services;

/// <summary>
///     Get pre-packaged raw data from the resources of this assembly
/// </summary>
public class RawDataService
{
    static readonly Dictionary<string, string> Versions = new()
    {
        { "steam-13390747", "steam-13390747.json" }
    };

    /// <summary>
    ///     A default version of the game. Use this value when an arbitrary version is needed.
    /// </summary>
    public const string DefaultVersion = "steam-13390747";

    readonly ILogger<RawDataService> _logger;
    readonly Dictionary<string, ExtractedData> _cachedData = new();

    /// <summary>
    ///     Create the raw data service
    /// </summary>
    public RawDataService(ILogger<RawDataService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Get all the available versions of the game.
    /// </summary>
    public IReadOnlyCollection<string> GetVersions() => Versions.Keys;

    /// <summary>
    ///     Get the data corresponding to the requested version of the game
    /// </summary>
    /// <param name="version">The requested version of the game</param>
    /// <returns>The extracted data from the requested version of the game</returns>
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
