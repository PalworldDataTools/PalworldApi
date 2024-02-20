namespace PalworldApi.Rest.v1.Models.Pals;

public enum PalSize
{
    /// <summary>
    ///     Unknown size
    /// </summary>
    Unknown,

    /// <summary>
    ///     The XS size
    /// </summary>
    XS,

    /// <summary>
    ///     The S size
    /// </summary>
    S,

    /// <summary>
    ///     The M size
    /// </summary>
    M,

    /// <summary>
    ///     The L size
    /// </summary>
    L,

    /// <summary>
    ///     The XL size
    /// </summary>
    XL
}

static class PalSizeMappingExtensions
{
    public static PalSize ToPalSize(this string size) =>
        size switch
        {
            "XS" => PalSize.XS,
            "S" => PalSize.S,
            "M" => PalSize.M,
            "L" => PalSize.L,
            "XL" => PalSize.XL,
            _ => PalSize.Unknown
        };
}
