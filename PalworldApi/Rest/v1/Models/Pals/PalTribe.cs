using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

public class PalTribe
{
    /// <summary>
    ///     The name of the tribe
    /// </summary>
    [Required] public required string Name { get; set; }

    /// <summary>
    ///     The pals in the tribe
    /// </summary>
    [Required] public required Pal[] Pals { get; set; }
}

static class PalTribeMappingExtensions
{
    public static PalTribe ToV1(this PalworldDataExtractor.Abstractions.Pals.PalTribe tribe) => new() { Name = tribe.Name, Pals = tribe.Pals.Select(p => p.ToV1()).ToArray() };
}
