using System.ComponentModel.DataAnnotations;

namespace PalworldApi.v1.Models.Pals;

public class PalWork
{
    /// <summary>
    ///     The craft speed of the pal
    /// </summary>
    [Required] public required int CraftSpeed { get; init; }

    /// <summary>
    ///     The speed of the pal when they transport object in a base
    /// </summary>
    [Required] public required int TransportSpeed { get; init; }

    /// <summary>
    ///     The level of the kindling role of the pal
    /// </summary>
    [Required] public required int Kindling { get; init; }

    /// <summary>
    ///     The level of the watering role of the pal
    /// </summary>
    [Required] public required int Watering { get; init; }

    /// <summary>
    ///     The level of the planting role of the pal
    /// </summary>
    [Required] public required int Planting { get; init; }

    /// <summary>
    ///     The level of the generating electricity role of the pal
    /// </summary>
    [Required] public required int GeneratingElectricity { get; init; }

    /// <summary>
    ///     The level of the handwork role of the pal
    /// </summary>
    [Required] public required int Handwork { get; init; }

    /// <summary>
    ///     The level of the gathering role of the pal
    /// </summary>
    [Required] public required int Gathering { get; init; }

    /// <summary>
    ///     The level of the lumbering role of the pal
    /// </summary>
    [Required] public required int Lumbering { get; init; }

    /// <summary>
    ///     The level of the mining role of the pal
    /// </summary>
    [Required] public required int Mining { get; init; }

    /// <summary>
    ///     The level of the oil extraction role of the pal
    /// </summary>
    [Required] public required int OilExtraction { get; init; }

    /// <summary>
    ///     The level of the medicine production role of the pal
    /// </summary>
    [Required] public required int MedicineProduction { get; init; }

    /// <summary>
    ///     The level of the cooling role of the pal
    /// </summary>
    [Required] public required int Cooling { get; init; }

    /// <summary>
    ///     The level of the transporting role of the pal
    /// </summary>
    [Required] public required int Transporting { get; init; }

    /// <summary>
    ///     The level of the farming role of the pal
    /// </summary>
    [Required] public required int Farming { get; init; }
}

public static class PalWorkMappingExtensions
{
    public static PalWork ToWorkV1(this PalworldDataExtractor.Models.Pals.Pal pal) =>
        new()
        {
            CraftSpeed = pal.CraftSpeed,
            TransportSpeed = pal.TransportSpeed,
            Kindling = pal.EmitFlame,
            Watering = pal.Watering,
            Planting = pal.Seeding,
            GeneratingElectricity = pal.GenerateElectricity,
            Handwork = pal.Handcraft,
            Gathering = pal.Collection,
            Lumbering = pal.Deforest,
            Mining = pal.Mining,
            OilExtraction = pal.OilExtraction,
            MedicineProduction = pal.ProduceMedicine,
            Cooling = pal.Cool,
            Transporting = pal.Transport,
            Farming = pal.MonsterFarm
        };
}
