using System.Text.Json;
using PalworldApi.Configuration;
using PalworldApi.Serialization;
using PalworldDataExtractor;
using PalworldDataExtractor.Models;

namespace PalworldApi.Services;

public class RawDataService
{
    readonly DataExtractionConfiguration _configuration;
    readonly ILogger<RawDataService> _logger;
    public ExtractedData Data { get; private set; } = null!;

    public RawDataService(DataExtractionConfiguration configuration, ILogger<RawDataService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Initialize()
    {
        DataExtractor extractor = new(
            _configuration.PalworldPakDirectory,
            config =>
            {
                config.MappingsFilePath = _configuration.ExtractorConfiguration.MappingsFilePath;
                config.UnrealEngineVersion = _configuration.ExtractorConfiguration.UnrealEngineVersion;
                config.PakFileName = _configuration.ExtractorConfiguration.PakFileName;
            }
        );

        _logger.LogDebug("Configuration: {config}", JsonSerializer.Serialize(_configuration, typeof(DataExtractionConfiguration), AppJsonSerializerContext.Default));
        _logger.LogInformation("Extracting data from .pak file...");

        Data = await extractor.Extract();

        _logger.LogInformation("Done extracting data.");
    }
}
