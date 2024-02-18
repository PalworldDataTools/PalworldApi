using System.Text.Json;
using System.Text.Json.Serialization;
using PalworldApi.Configuration;
using PalworldDataExtractor.Models.Pals;

namespace PalworldApi.Serialization;

#if DEBUG
[JsonSourceGenerationOptions(JsonSerializerDefaults.Web, WriteIndented = true)]
#else
[JsonSourceGenerationOptions(JsonSerializerDefaults.Web, WriteIndented = false)]
#endif
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(IEnumerable<string>))]
[JsonSerializable(typeof(DataExtractionConfiguration))]
[JsonSerializable(typeof(Pal))]
partial class AppJsonSerializerContext : JsonSerializerContext
{
}
