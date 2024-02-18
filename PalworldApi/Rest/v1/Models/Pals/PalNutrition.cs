using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

public class PalNutrition
{
    /// <summary>
    ///     The size of the stomach of the pal
    /// </summary>
    [Required] public required int MaxFullStomach { get; set; }

    /// <summary>
    ///     The rate at which the stomach of the pal empties when it is full
    /// </summary>
    [Required] public required float FullStomachDecreaseRate { get; set; }

    /// <summary>
    ///     The food amount of the pal
    /// </summary>
    [Required] public required int FoodAmount { get; set; }
}

public static class PalNutritionMappingExtensions
{
    public static PalNutrition ToNutritionV1(this PalworldDataExtractor.Models.Pals.Pal pal) =>
        new()
        {
            MaxFullStomach = pal.MaxFullStomach,
            FullStomachDecreaseRate = pal.FullStomachDecreaseRate,
            FoodAmount = pal.FoodAmount
        };
}
