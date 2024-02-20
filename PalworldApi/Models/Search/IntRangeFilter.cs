namespace PalworldApi.Models.Search;

/// <summary>
///     Range of integers used in search requests
/// </summary>
public class IntRangeFilter
{
    /// <summary>
    ///     Lower bound of the range
    /// </summary>
    public int? FromInclusive { get; set; }

    /// <summary>
    ///     Upper bound of the range
    /// </summary>
    public int? ToInclusive { get; set; }
}

static class IntRangeFilterExtensions
{
    public static bool Contains(this IntRangeFilter filter, int value) =>
        (!filter.FromInclusive.HasValue || value >= filter.FromInclusive) && (!filter.ToInclusive.HasValue || value <= filter.ToInclusive);
}
