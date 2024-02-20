using PalworldApi.Rest.OpenApi;

namespace PalworldApi.Models.Search;

/// <summary>
///     Pagination parameters of a search
/// </summary>
[IncludeInOpenApi]
public class PaginationRequest
{
    /// <summary>
    ///     The number of the page that is requested
    /// </summary>
    public int? PageNumber { get; set; }

    /// <summary>
    ///     The number of results per page
    /// </summary>
    public int? PageSize { get; set; }
}
