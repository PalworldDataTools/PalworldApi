using PalworldDataExtractor;

namespace PalworldApi.Configuration;

public class DataExtractionConfiguration
{
    public required string PalworldPakDirectory { get; init; }
    public required DataExtractorConfiguration ExtractorConfiguration { get; init; }
}
