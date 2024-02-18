namespace PalworldApi.Rest.v1.Models.Pals;

public enum PalElement
{
    Unknown,
    Dark,
    Dragon,
    Earth,
    Electricity,
    Fire,
    Ice,
    Leaf,
    Normal,
    Water
}

public static class PalElementMappingExtensions
{
    public static PalElement ToPalElement(this string size) =>
        size switch
        {
            "Dark" => PalElement.Dark,
            "Dragon" => PalElement.Dragon,
            "Earth" => PalElement.Earth,
            "Electricity" => PalElement.Electricity,
            "Fire" => PalElement.Fire,
            "Ice" => PalElement.Ice,
            "Leaf" => PalElement.Leaf,
            "Normal" => PalElement.Normal,
            "Water" => PalElement.Water,
            _ => PalElement.Unknown
        };
}
