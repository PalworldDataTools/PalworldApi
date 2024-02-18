namespace PalworldApi.Rest.v1.Models.Pals;

public enum PalSize
{
    Unknown,
    XS,
    S,
    M,
    L,
    XL
}

public static class PalSizeMappingExtensions
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
