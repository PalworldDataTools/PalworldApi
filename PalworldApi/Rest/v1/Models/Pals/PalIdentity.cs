using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Rest.v1.Models.Pals;

public class PalIdentity
{
    /// <summary>
    ///     The name of the tribe to which the pal belongs
    /// </summary>
    [Required] public required string TribeName { get; set; }

    /// <summary>
    ///     The unique ID of the pal
    /// </summary>
    [Required] public required string Name { get; set; }

    /// <summary>
    ///     The nice representation of the name of the pal
    /// </summary>
    [Required] public required string DisplayName { get; set; }
}

public static class PalIdentityMappingExtensions
{
    public static PalIdentity ToIdentityV1(this PalworldDataExtractor.Models.Pals.Pal pal) =>
        new()
        {
            TribeName = pal.TribeName ?? "??",
            Name = pal.Name,
            DisplayName = pal.DisplayName
        };
}
