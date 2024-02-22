using System.ComponentModel.DataAnnotations;
using PalworldApi.Services;

namespace PalworldApi.Rest.v1.Models.Pals;

/// <summary>
///     Couple of pals
/// </summary>
public class PalCouple
{
    /// <summary>
    ///     The first pal of the couple
    /// </summary>
    [Required] public required Pal PalA { get; set; }

    /// <summary>
    ///     The second pal of the couple
    /// </summary>
    [Required] public required Pal PalB { get; set; }
}

static class PalCoupleMappingExtensions
{
    public static PalCouple ToV1(this Requests.Breeding.PalCouple couple, Localizer? localizer = null) =>
        new() { PalA = couple.PalA.ToV1(localizer), PalB = couple.PalB.ToV1(localizer) };
}
