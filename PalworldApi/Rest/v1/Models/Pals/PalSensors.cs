using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

/// <summary>
///     Sensors statistics of a pal
/// </summary>
public class PalSensors
{
    /// <summary>
    ///     The distance at which the pal can view other entities
    /// </summary>
    [Required] public required int ViewingDistance { get; set; }

    /// <summary>
    ///     The angle at which the pal can view other entities
    /// </summary>
    [Required] public required int ViewingAngle { get; set; }

    /// <summary>
    ///     The hearing rate of the pal
    /// </summary>
    [Required] public required float HearingRate { get; set; }
}

static class PalSensorsMappingExtensions
{
    public static PalSensors ToSensorsV1(this PalworldDataExtractor.Abstractions.Pals.Pal pal) =>
        new()
        {
            ViewingDistance = pal.ViewingDistance,
            ViewingAngle = pal.ViewingAngle,
            HearingRate = pal.HearingRate
        };
}
