using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

/// <summary>
///     Breeding statistics of a pal
/// </summary>
public class PalBreeding
{
    /// <summary>
    ///     The probability of getting a male when breeding the pal
    /// </summary>
    [Required] public required float MaleProbability { get; set; }

    /// <summary>
    ///     The breeding rank of the pal.
    ///     The breeding rank is used to find the result of breeding two pals together. The child of two pals is the pal whose breeding rank is the closest to the average rank of
    ///     its parents.
    /// </summary>
    [Required] public required int BreedingRank { get; set; }
}

static class PalBreedingMappingExtensions
{
    public static PalBreeding ToBreedingV1(this PalworldDataExtractor.Abstractions.Pals.Pal pal) =>
        new()
        {
            MaleProbability = pal.MaleProbability,
            BreedingRank = pal.CombiRank
        };
}
