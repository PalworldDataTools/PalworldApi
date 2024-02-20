using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

/// <summary>
///     Tribe of pals. A tribe is a collection of pal variants that look the same but vary in statistics.
///     For example, there might be a pal, its boss variant and its gym boss variant in the same tribe.
///     There could also be variants of different elements with different spells that are meant to live in different biomes.
/// </summary>
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
