using System.ComponentModel.DataAnnotations;

namespace PalworldApi.Models.Search;

/// <summary>
///     The result of a search
/// </summary>
public class SearchResult<TResult>
{
    /// <summary>
    ///     The current page of results
    /// </summary>
    [Required] public required IReadOnlyCollection<TResult> Results { get; set; }

    /// <summary>
    ///     The pagination information of the result
    /// </summary>
    [Required] public required PaginationResult Pagination { get; set; }
}

static class SearchResultMappingExtensions
{
    public static SearchResult<TDestination> Select<TSource, TDestination>(this SearchResult<TSource> sourceResults, Func<TSource, TDestination> selector) =>
        new()
        {
            Results = sourceResults.Results.Select(selector).ToArray(),
            Pagination = sourceResults.Pagination
        };
}
