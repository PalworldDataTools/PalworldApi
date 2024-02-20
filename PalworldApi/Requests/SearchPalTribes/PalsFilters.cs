using PalworldApi.Models.Search;
using PalworldApi.Rest.OpenApi;
using PalworldApi.Rest.v1.Models.Pals;

namespace PalworldApi.Requests.SearchPalTribes;

/// <summary>
///     Filter parameters used to search pals
/// </summary>
[IncludeInOpenApi]
public class PalsFilters
{
    /// <summary>
    ///     The sizes of the resulting pals.
    /// </summary>
    public PalSize[]? Sizes { get; set; }

    /// <summary>
    ///     The elements that the resulting pals can have, either as first or second element.
    /// </summary>
    public PalElement[]? Elements { get; set; }

    /// <summary>
    ///     Should the resulting pals have a nocturnal variant.
    /// </summary>
    public bool? HasNocturnalVariant { get; set; }

    /// <summary>
    ///     Should the resulting pals have a edible variant.
    /// </summary>
    public bool? HasEdibleVariant { get; set; }

    /// <summary>
    ///     Should the resulting pals have a predator variant.
    /// </summary>
    public bool? HasPredatorVariant { get; set; }

    /// <summary>
    ///     Should the resulting pals have a boss variant.
    /// </summary>
    public bool? HasBossVariant { get; set; }

    /// <summary>
    ///     Should the resulting pals have a gym boss variant.
    /// </summary>
    public bool? HasGymBossVariant { get; set; }

    /// <summary>
    ///     The range of rarities of the resulting pals.
    /// </summary>
    public IntRangeFilter? Rarity { get; set; }

    /// <summary>
    ///     The work suitability filter parameters
    /// </summary>
    public PalsWorkSuitabilityFilters? WorkSuitability { get; set; }
}
