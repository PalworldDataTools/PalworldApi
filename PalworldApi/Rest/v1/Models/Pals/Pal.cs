﻿using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

public class Pal
{
    /// <summary>
    ///     The identity of the pal
    /// </summary>
    [Required] public required PalIdentity Identity { get; set; }

    /// <summary>
    ///     Is the pal a boss. Bosses can be found in specific locations in the world or in gyms
    /// </summary>
    [Required] public required bool IsBoss { get; set; }

    /// <summary>
    ///     Is the pal the boss of a gym
    /// </summary>
    [Required] public required bool IsGymBoss { get; set; }

    /// <summary>
    ///     Is the pal active at night time
    /// </summary>
    [Required] public required bool IsNocturnal { get; set; }

    /// <summary>
    ///     Is the pal edible. If the pal is edible, there remains will be eaten by nearby predator pals when they die
    /// </summary>
    [Required] public required bool IsEdible { get; set; }

    /// <summary>
    ///     Is the pal a predator. If the pal is a predator, they will eat the remains of nearby edible pals after they die
    /// </summary>
    [Required] public required bool IsPredator { get; set; }

    /// <summary>
    ///     The first element type of the pal
    /// </summary>
    [Required] public required PalElement Element1 { get; set; }

    /// <summary>
    ///     The second element type of the pal, if any
    /// </summary>
    public PalElement? Element2 { get; set; }

    /// <summary>
    ///     The statistics of the pal
    /// </summary>
    [Required] public required PalStatistics Statistics { get; set; }

    /// <summary>
    ///     The sensors of the pal
    /// </summary>
    [Required] public required PalSensors Sensors { get; set; }

    /// <summary>
    ///     The nutrition statistics of the pal
    /// </summary>
    [Required] public required PalNutrition Nutrition { get; set; }

    /// <summary>
    ///     The combat statistics of the pal
    /// </summary>
    [Required] public required PalCombat Combat { get; set; }

    /// <summary>
    ///     The breeding statistics of the pal
    /// </summary>
    [Required] public required PalBreeding Breeding { get; set; }

    /// <summary>
    ///     The work statistics of the pal
    /// </summary>
    [Required] public required PalWork Work { get; set; }
}

public static class PalMappingExtensions
{
    public static Pal ToV1(PalworldDataExtractor.Models.Pals.Pal pal) =>
        new()
        {
            Identity = pal.ToIdentityV1(),
            IsBoss = pal.IsBoss,
            IsGymBoss = pal.IsTowerBoss,
            IsNocturnal = pal.IsNocturnal,
            IsEdible = pal.IsEdible,
            IsPredator = pal.IsPredator,
            Element1 = pal.ElementType1.ToPalElement(),
            Element2 = pal.ElementType2 == "None" ? null : pal.ElementType2.ToPalElement(),
            Statistics = pal.ToStatisticsV1(),
            Sensors = pal.ToSensorsV1(),
            Nutrition = pal.ToNutritionV1(),
            Combat = pal.ToCombatV1(),
            Breeding = pal.ToBreedingV1(),
            Work = pal.ToWorkV1()
        };
}