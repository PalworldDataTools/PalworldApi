namespace PalworldApi.Models.Search;

/// <summary>
///     Range of floats used in search requests
/// </summary>
public class FloatRangeFilter
{
    /// <summary>
    ///     Lower bound of the range
    /// </summary>
    public float? FromInclusive { get; set; }

    /// <summary>
    ///     Upper bound of the range
    /// </summary>
    public float? ToInclusive { get; set; }
}

static class FloatRangeFilterExtensions
{
    public static bool Contains(this FloatRangeFilter filter, float value) =>
        (!filter.FromInclusive.HasValue || value >= filter.FromInclusive) && (!filter.ToInclusive.HasValue || value <= filter.ToInclusive);
}
