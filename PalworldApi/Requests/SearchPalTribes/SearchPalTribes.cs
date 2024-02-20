using MediatR;
using PalworldApi.Models.Search;
using PalworldApi.Rest.v1.Models.Pals;
using PalworldApi.Services;
using PalTribe = PalworldDataExtractor.Abstractions.Pals.PalTribe;

namespace PalworldApi.Requests.SearchPalTribes;

class SearchPalTribes : IRequestHandler<SearchPalTribesRequest, SearchResult<PalTribe>>
{
    readonly RawDataService _rawDataService;

    public SearchPalTribes(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    public async Task<SearchResult<PalTribe>> Handle(SearchPalTribesRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<PalTribe> result = request.Data.Tribes;

        if (request.SearchRequest.Filter != null)
        {
            result = Filter(result, request.SearchRequest.Filter);
        }

        return SearchUtils.Paginate(result, request.SearchRequest.Pagination);
    }

    static IEnumerable<PalTribe> Filter(IEnumerable<PalTribe> result, PalsFilters filters)
    {
        if (filters.Sizes != null)
        {
            result = result.Where(tribe => tribe.Pals.Any(p => filters.Sizes.Contains(p.Size.ToPalSize())));
        }

        if (filters.Elements != null)
        {
            result = result.Where(
                tribe => tribe.Pals.Any(
                    p =>
                    {
                        PalElement element1 = p.ElementType1.ToPalElement();
                        if (element1 != PalElement.Unknown && filters.Elements.Contains(element1))
                        {
                            return true;
                        }

                        PalElement element2 = p.ElementType2.ToPalElement();
                        if (element2 != PalElement.Unknown && filters.Elements.Contains(element2))
                        {
                            return true;
                        }

                        return false;
                    }
                )
            );
        }

        if (filters.HasNocturnalVariant.HasValue)
        {
            result = result.Where(tribe => tribe.Pals.Any(p => p.IsNocturnal == filters.HasNocturnalVariant));
        }

        if (filters.HasEdibleVariant.HasValue)
        {
            result = result.Where(tribe => tribe.Pals.Any(p => p.IsEdible == filters.HasEdibleVariant));
        }

        if (filters.HasPredatorVariant.HasValue)
        {
            result = result.Where(tribe => tribe.Pals.Any(p => p.IsPredator == filters.HasPredatorVariant));
        }

        if (filters.HasBossVariant.HasValue)
        {
            result = result.Where(tribe => tribe.Pals.Any(p => p.IsBoss == filters.HasBossVariant));
        }

        if (filters.HasGymBossVariant.HasValue)
        {
            result = result.Where(tribe => tribe.Pals.Any(p => p.IsTowerBoss == filters.HasGymBossVariant));
        }

        if (filters.Rarity != null)
        {
            result = result.Where(tribe => tribe.Pals.Any(p => filters.Rarity.Contains(p.Rarity)));
        }

        if (filters.WorkSuitability != null)
        {
            if (filters.WorkSuitability.Kindling != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Kindling.Contains(p.EmitFlame)));
            }

            if (filters.WorkSuitability.Watering != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Watering.Contains(p.Watering)));
            }

            if (filters.WorkSuitability.Planting != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Planting.Contains(p.Seeding)));
            }

            if (filters.WorkSuitability.GeneratingElectricity != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.GeneratingElectricity.Contains(p.GenerateElectricity)));
            }

            if (filters.WorkSuitability.Handwork != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Handwork.Contains(p.Handcraft)));
            }

            if (filters.WorkSuitability.Gathering != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Gathering.Contains(p.Collection)));
            }

            if (filters.WorkSuitability.Lumbering != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Lumbering.Contains(p.Deforest)));
            }

            if (filters.WorkSuitability.Mining != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Mining.Contains(p.Mining)));
            }

            if (filters.WorkSuitability.OilExtraction != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.OilExtraction.Contains(p.OilExtraction)));
            }

            if (filters.WorkSuitability.MedicineProduction != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.MedicineProduction.Contains(p.ProduceMedicine)));
            }

            if (filters.WorkSuitability.Cooling != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Cooling.Contains(p.Cool)));
            }

            if (filters.WorkSuitability.Transporting != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Transporting.Contains(p.Transport)));
            }

            if (filters.WorkSuitability.Farming != null)
            {
                result = result.Where(tribe => tribe.Pals.Any(p => filters.WorkSuitability.Farming.Contains(p.MonsterFarm)));
            }
        }

        return result;
    }
}
