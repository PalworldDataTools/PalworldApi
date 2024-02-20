namespace PalworldApi.Models.Search;

/// <summary>
///     Parameters of a search
/// </summary>
public class SearchRequest<TFilter>
{
    /// <summary>
    ///     The filter parameters of the search
    /// </summary>
    public TFilter? Filter { get; set; }

    /// <summary>
    ///     The pagination parameters of the search
    /// </summary>
    public PaginationRequest? Pagination { get; set; }
}
