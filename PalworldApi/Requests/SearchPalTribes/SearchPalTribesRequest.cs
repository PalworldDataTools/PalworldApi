using MediatR;
using PalworldApi.Models;
using PalworldApi.Models.Search;
using PalTribe = PalworldDataExtractor.Abstractions.Pals.PalTribe;

namespace PalworldApi.Requests.SearchPalTribes;

class SearchPalTribesRequest : IRequest<SearchResult<PalTribe>>
{
    public required VersionedData Data { get; init; }
    public required SearchRequest<PalsFilters> SearchRequest { get; init; }
}
