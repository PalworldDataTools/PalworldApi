using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

public class PalCombat
{
    /// <summary>
    ///     The HPs of the pal
    /// </summary>
    [Required] public required int Hp { get; set; }

    /// <summary>
    ///     The melee attack of the pal
    /// </summary>
    [Required] public required int MeleeAttack { get; set; }

    /// <summary>
    ///     The shot attack of the pal
    /// </summary>
    [Required] public required int ShotAttack { get; set; }

    /// <summary>
    ///     The defense of the pal
    /// </summary>
    [Required] public required int Defense { get; set; }

    /// <summary>
    ///     The support of the pal
    /// </summary>
    [Required] public required int Support { get; set; }
}

public static class PalCombatMappingExtensions
{
    public static PalCombat ToCombatV1(this PalworldDataExtractor.Models.Pals.Pal pal) =>
        new()
        {
            Hp = pal.Hp,
            MeleeAttack = pal.MeleeAttack,
            ShotAttack = pal.ShotAttack,
            Defense = pal.Defense,
            Support = pal.Support
        };
}
