using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Models.Search;

/// <summary>
///     Pagination information of a search result
/// </summary>
public class PaginationResult
{
    /// <summary>
    ///     The current page number
    /// </summary>
    [Required] public required int PageNumber { get; set; }

    /// <summary>
    ///     The number of results per page
    /// </summary>
    [Required] public required int PageSize { get; set; }

    /// <summary>
    ///     The total number of results
    /// </summary>
    [Required] public required int TotalCount { get; set; }

    /// <summary>
    ///     The total number of pages
    /// </summary>
    [Required] public required int TotalPages { get; set; }
}
