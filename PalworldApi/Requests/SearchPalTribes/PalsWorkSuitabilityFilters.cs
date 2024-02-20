using PalworldApi.Models.Search;

namespace PalworldApi.Requests.SearchPalTribes;

/// <summary>
///     Work suitability filter parameters used to search pals
/// </summary>
public class PalsWorkSuitabilityFilters
{
    /// <summary>
    ///     The range of kindling role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Kindling { get; set; }

    /// <summary>
    ///     The range of watering role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Watering { get; set; }

    /// <summary>
    ///     The range of planting role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Planting { get; set; }

    /// <summary>
    ///     The range of generating electricity role of the resulting pals.
    /// </summary>
    public IntRangeFilter? GeneratingElectricity { get; set; }

    /// <summary>
    ///     The range of handwork role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Handwork { get; set; }

    /// <summary>
    ///     The range of gathering role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Gathering { get; set; }

    /// <summary>
    ///     The range of lumbering role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Lumbering { get; set; }

    /// <summary>
    ///     The range of mining role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Mining { get; set; }

    /// <summary>
    ///     The range of oil extraction role of the resulting pals.
    /// </summary>
    public IntRangeFilter? OilExtraction { get; set; }

    /// <summary>
    ///     The range of medicine production role of the resulting pals.
    /// </summary>
    public IntRangeFilter? MedicineProduction { get; set; }

    /// <summary>
    ///     The range of cooling role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Cooling { get; set; }

    /// <summary>
    ///     The range of transporting role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Transporting { get; set; }

    /// <summary>
    ///     The range of farming role of the resulting pals.
    /// </summary>
    public IntRangeFilter? Farming { get; set; }
}
