using MediatR;
using PalworldApi.Models.Search;
using PalworldDataExtractor.Abstractions;
using PalTribe = PalworldDataExtractor.Abstractions.Pals.PalTribe;

namespace PalworldApi.Requests.SearchPalTribes;

class SearchPalTribesRequest : IRequest<SearchResult<PalTribe>>
{
    public required SearchRequest<PalsFilters> SearchRequest { get; init; }
    public required ExtractedData Data { get; init; }
}
