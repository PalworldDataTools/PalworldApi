using PalworldApi.Models;
using PalworldDataExtractor.Abstractions.L10N;

namespace PalworldApi.Services;

/// <summary>
///     Localize strings using the localization files of the game
/// </summary>
public class LocalizationService
{
    public const string DefaultLanguage = "en";

    readonly RawDataService _rawDataService;
    readonly Dictionary<string, Localizer> _cachedLocalizers = new();

    /// <summary>
    ///     Create the localization service
    /// </summary>
    /// <param name="rawDataService"></param>
    public LocalizationService(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    /// <summary>
    ///     Get all the available languages for the given version. If no version is given, the <see cref="RawDataService.DefaultVersion" /> is used instead.
    /// </summary>
    public async Task<IReadOnlyCollection<string>> GetLanguages(string? version = null)
    {
        VersionedData? data = await _rawDataService.GetData(version ?? RawDataService.DefaultVersion);
        if (data == null)
        {
            return Array.Empty<string>();
        }

        return data.Data.LocalizationFiles.Keys.ToArray();
    }

    /// <summary>
    ///     Create an encapsulated localization context for the given language and the given version.
    ///     If no version is given, the <see cref="RawDataService.DefaultVersion" /> is used instead.
    /// </summary>
    public async Task<Localizer?> GetLocalizer(string language, string? version = null)
    {
        if (_cachedLocalizers.TryGetValue(language, out Localizer? cachedLocalizer))
        {
            return cachedLocalizer;
        }

        VersionedData? data = await _rawDataService.GetData(version ?? RawDataService.DefaultVersion);
        if (data == null)
        {
            return null;
        }

        if (!data.Data.LocalizationFiles.TryGetValue(language, out LocalizationFile? file))
        {
            return null;
        }

        Localizer localizer = new(file);
        _cachedLocalizers[language] = localizer;

        return localizer;
    }

    /// <summary>
    ///     Localize the given string in the given language and the given version. If no version is given, the <see cref="RawDataService.DefaultVersion" /> is used instead.
    /// </summary>
    public async Task<string?> Localize(string key, string language, string? version = null)
    {
        Localizer? localizer = await GetLocalizer(language, language);
        return localizer?.Localize(key);
    }
}

/// <summary>
///     Encapsulate a localization context and provide the <see cref="Localize" /> method
/// </summary>
public class Localizer
{
    readonly Dictionary<string, string> _mapping;

    /// <summary>
    ///     Create the localizer that encapsulates the given localization file
    /// </summary>
    public Localizer(LocalizationFile file)
    {
        // comparisons need to be case insensitive because there are mistakes in the game files
        // the easiest way is to reconstruct the dictionaries with normalized keys
        // and since we're reconstructing anyway, we can also flatten the dictionaries to make lookups easier later on
        _mapping = file.Namespaces.Select(kv => kv.Value)
            .SelectMany(ns => ns.Fields.Select(kv => new { ns.Namespace, Field = kv.Key, kv.Value }))
            .ToDictionary(x => Normalize($"{x.Namespace}.{x.Field}"), x => x.Value);
    }

    /// <summary>
    ///     Localize the given string.
    ///     The key should look like Namespace.Field, e.g. the key for the name of the pal Alpaca, the key is DT_PalNameText.PAL_NAME_Alpaca.
    /// </summary>
    public string? Localize(string key) => _mapping.GetValueOrDefault(Normalize(key));

    static string Normalize(string key) => key.ToLowerInvariant();
}
