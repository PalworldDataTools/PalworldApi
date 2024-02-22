using System.ComponentModel.DataAnnotations;
using PalworldApi.Services;

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
    ///     The localized name of the tribe
    /// </summary>
    public string? LocalizedName { get; set; }

    /// <summary>
    ///     The pals in the tribe
    /// </summary>
    [Required] public required Pal[] Pals { get; set; }
}

static class PalTribeMappingExtensions
{
    public static PalTribe ToV1(this PalworldDataExtractor.Abstractions.Pals.PalTribe tribe, Localizer? localizer = null) =>
        new() { Name = tribe.Name, LocalizedName = localizer?.Localize($"DT_PalNameText.PAL_NAME_{tribe.Name}"), Pals = tribe.Pals.Select(p => p.ToV1(localizer)).ToArray() };
}

static class PalTribeExtensions
{
    public static PalworldDataExtractor.Abstractions.Pals.Pal GetMainPal(this PalworldDataExtractor.Abstractions.Pals.PalTribe tribe) =>
        tribe.Pals.FirstOrDefault(p => p is { IsBoss: false, IsTowerBoss: false }) ?? tribe.Pals.First();

    public static PalworldDataExtractor.Abstractions.Pals.Pal? GetBossPal(this PalworldDataExtractor.Abstractions.Pals.PalTribe tribe) =>
        tribe.Pals.FirstOrDefault(p => p is { IsBoss: true, IsTowerBoss: false });

    public static PalworldDataExtractor.Abstractions.Pals.Pal? GetGymPal(this PalworldDataExtractor.Abstractions.Pals.PalTribe tribe) =>
        tribe.Pals.FirstOrDefault(p => p is { IsTowerBoss: true });
}
