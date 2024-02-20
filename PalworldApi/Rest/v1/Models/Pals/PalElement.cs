namespace PalworldApi.Rest.v1.Models.Pals;

/// <summary>
///     Elements that pals or skills can have
/// </summary>
public enum PalElement
{
    /// <summary>
    ///     Unknown element
    /// </summary>
    Unknown,

    /// <summary>
    ///     The dark element
    /// </summary>
    Dark,

    /// <summary>
    ///     The dragon element
    /// </summary>
    Dragon,

    /// <summary>
    ///     The earth element
    /// </summary>
    Earth,

    /// <summary>
    ///     The electricity element
    /// </summary>
    Electricity,

    /// <summary>
    ///     The fire element
    /// </summary>
    Fire,

    /// <summary>
    ///     The ice element
    /// </summary>
    Ice,

    /// <summary>
    ///     The leaf element
    /// </summary>
    Leaf,

    /// <summary>
    ///     The normal element
    /// </summary>
    Normal,

    /// <summary>
    ///     The water element
    /// </summary>
    Water
}

static class PalElementMappingExtensions
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
