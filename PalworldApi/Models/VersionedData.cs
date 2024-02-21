using PalworldDataExtractor.Abstractions;

namespace PalworldApi.Models;

/// <summary>
///     Extracted data with their version
/// </summary>
public class VersionedData
{
    /// <summary>
    ///     The version of the data
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    ///     The extracted data
    /// </summary>
    public required ExtractedData Data { get; set; }
}
