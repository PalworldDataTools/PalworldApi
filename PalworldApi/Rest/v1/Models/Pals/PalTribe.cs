using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

public class PalTribe
{
    [Required] public required string Name { get; set; }
    [Required] public required Pal[] Pals { get; set; }
}

public static class PalTribeMappingExtensions
{
    public static PalTribe ToV1(this PalworldDataExtractor.Abstractions.Pals.PalTribe tribe) => new() { Name = tribe.Name, Pals = tribe.Pals.Select(p => p.ToV1()).ToArray() };
}
