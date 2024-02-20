using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

/// <summary>
///     Statistics of a pal
/// </summary>
public class PalStatistics
{
    /// <summary>
    ///     The size of the pal
    /// </summary>
    [Required] public required PalSize Size { get; set; }

    /// <summary>
    ///     The rarity of the pal, from 1 to 10
    /// </summary>
    [Required] public required int Rarity { get; set; }

    /// <summary>
    ///     The multiplier applied to the experience gained by the pal
    /// </summary>
    [Required] public required float ExpRatio { get; set; }

    /// <summary>
    ///     The stamina of the pal. It affects how long a pal can perform actions.
    /// </summary>
    [Required] public required int Stamina { get; set; }

    /// <summary>
    ///     The speed of the pal when they walk slowly
    /// </summary>
    [Required] public required int SlowWalkSpeed { get; set; }

    /// <summary>
    ///     The speed of the pal when they walk at a normal pace
    /// </summary>
    [Required] public required int WalkSpeed { get; set; }

    /// <summary>
    ///     The speed of the pal when they run
    /// </summary>
    [Required] public required int RunSpeed { get; set; }

    /// <summary>
    ///     The speed of the pal when they are used as a ride
    /// </summary>
    [Required] public required int RideSprintSpeed { get; set; }

    /// <summary>
    ///     The capture rate of the pal
    /// </summary>
    [Required] public required float CaptureRate { get; set; }

    /// <summary>
    ///     The price of the pal when they are being sold by a merchant
    /// </summary>
    [Required] public required float Price { get; set; }
}

static class PalStatisticsMappingExtensions
{
    public static PalStatistics ToStatisticsV1(this PalworldDataExtractor.Abstractions.Pals.Pal pal) =>
        new()
        {
            Size = pal.Size.ToPalSize(),
            Rarity = pal.Rarity,
            ExpRatio = pal.ExpRatio,
            Stamina = pal.Stamina,
            SlowWalkSpeed = pal.SlowWalkSpeed,
            WalkSpeed = pal.WalkSpeed,
            RunSpeed = pal.RunSpeed,
            RideSprintSpeed = pal.RideSprintSpeed,
            CaptureRate = pal.CaptureRate,
            Price = pal.Price
        };
}
