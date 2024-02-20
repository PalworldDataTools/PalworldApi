namespace PalworldApi.Models.Search;

static class SearchUtils
{
    const int DefaultPageSize = 10;

    public static SearchResult<TResult> Paginate<TResult>(IEnumerable<TResult> allResults, PaginationRequest? request)
    {
        TResult[] enumeratedResults = allResults as TResult[] ?? allResults.ToArray();
        int count = enumeratedResults.Length;

        int? pageSize = request?.PageSize ?? DefaultPageSize;
        if (pageSize is null or <= 0)
        {
            return new SearchResult<TResult>
            {
                Results = Array.Empty<TResult>(),
                Pagination = new PaginationResult
                {
                    PageNumber = 0,
                    PageSize = 0,
                    TotalCount = count,
                    TotalPages = 0
                }
            };
        }

        int pageNumber = request?.PageNumber is > 0 ? request.PageNumber.Value : 1;
        int toSkip = (pageNumber - 1) * pageSize.Value;

        return new SearchResult<TResult>
        {
            Results = enumeratedResults.Skip(toSkip).Take(pageSize.Value).ToArray(),
            Pagination = new PaginationResult
            {
                PageNumber = pageNumber,
                PageSize = pageSize.Value,
                TotalCount = count,
                TotalPages = Convert.ToInt32(Math.Ceiling((float)count / pageSize.Value))
            }
        };
    }
}
