using MediatR;
using PalworldApi.Models.Search;
using PalworldApi.Rest.v1.Models.Pals;
using Pal = PalworldDataExtractor.Abstractions.Pals.Pal;
using PalTribe = PalworldDataExtractor.Abstractions.Pals.PalTribe;

namespace PalworldApi.Requests.SearchPalTribes;

class SearchPalTribes : IRequestHandler<SearchPalTribesRequest, SearchResult<PalTribe>>
{
    public Task<SearchResult<PalTribe>> Handle(SearchPalTribesRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<PalTribe> result = request.Data.Tribes;

        if (request.SearchRequest.Filter != null)
        {
            result = Filter(result, request.SearchRequest.Filter);
        }

        return Task.FromResult(SearchUtils.Paginate(result, request.SearchRequest.Pagination));
    }

    static IEnumerable<PalTribe> Filter(IEnumerable<PalTribe> result, PalsFilters filters)
    {
        if (filters.Sizes != null)
        {
            result = result.Where(AtLeastOneVariantHasValueInCollection(p => p.Size.ToPalSize(), filters.Sizes));
        }

        if (filters.Elements != null)
        {
            result = result.Where(AtLeastOneVariantHasAtLeastOneValueInCollection(p => [p.ElementType1.ToPalElement(), p.ElementType2.ToPalElement()], filters.Elements));
        }

        if (filters.HasNocturnalVariant.HasValue)
        {
            result = result.Where(AtLeastOneVariantSatisfies(p => p.IsNocturnal, filters.HasNocturnalVariant.Value));
        }

        if (filters.HasEdibleVariant.HasValue)
        {
            result = result.Where(AtLeastOneVariantSatisfies(p => p.IsEdible, filters.HasEdibleVariant.Value));
        }

        if (filters.HasPredatorVariant.HasValue)
        {
            result = result.Where(AtLeastOneVariantSatisfies(p => p.IsPredator, filters.HasPredatorVariant.Value));
        }

        if (filters.HasBossVariant.HasValue)
        {
            result = result.Where(AtLeastOneVariantSatisfies(p => p.IsBoss, filters.HasBossVariant.Value));
        }

        if (filters.HasGymBossVariant.HasValue)
        {
            result = result.Where(AtLeastOneVariantSatisfies(p => p.IsTowerBoss, filters.HasGymBossVariant.Value));
        }

        if (filters.Rarity != null)
        {
            result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Rarity, filters.Rarity));
        }

        if (filters.WorkSuitability != null)
        {
            if (filters.WorkSuitability.Kindling != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.EmitFlame, filters.WorkSuitability.Kindling));
            }

            if (filters.WorkSuitability.Watering != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Watering, filters.WorkSuitability.Watering));
            }

            if (filters.WorkSuitability.Planting != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Seeding, filters.WorkSuitability.Planting));
            }

            if (filters.WorkSuitability.GeneratingElectricity != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.GenerateElectricity, filters.WorkSuitability.GeneratingElectricity));
            }

            if (filters.WorkSuitability.Handwork != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Handcraft, filters.WorkSuitability.Handwork));
            }

            if (filters.WorkSuitability.Gathering != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Collection, filters.WorkSuitability.Gathering));
            }

            if (filters.WorkSuitability.Lumbering != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Deforest, filters.WorkSuitability.Lumbering));
            }

            if (filters.WorkSuitability.Mining != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Mining, filters.WorkSuitability.Mining));
            }

            if (filters.WorkSuitability.OilExtraction != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.OilExtraction, filters.WorkSuitability.OilExtraction));
            }

            if (filters.WorkSuitability.MedicineProduction != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.ProduceMedicine, filters.WorkSuitability.MedicineProduction));
            }

            if (filters.WorkSuitability.Cooling != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Cool, filters.WorkSuitability.Cooling));
            }

            if (filters.WorkSuitability.Transporting != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.Transport, filters.WorkSuitability.Transporting));
            }

            if (filters.WorkSuitability.Farming != null)
            {
                result = result.Where(AtLeastOneVariantHasValueInRange(p => p.MonsterFarm, filters.WorkSuitability.Farming));
            }
        }

        return result;
    }

    static Func<PalTribe, bool> AtLeastOneVariantHasValueInRange(Func<Pal, int> getValue, IntRangeFilter range) => AtLeastOneVariantSatisfies(p => range.Contains(getValue(p)));

    static Func<PalTribe, bool> AtLeastOneVariantHasValueInCollection<TValue>(Func<Pal, TValue> getValue, TValue[] collection) =>
        AtLeastOneVariantSatisfies(p => collection.Contains(getValue(p)));

    static Func<PalTribe, bool> AtLeastOneVariantHasAtLeastOneValueInCollection<TValue>(Func<Pal, TValue[]> getValues, TValue[] collection) =>
        AtLeastOneVariantSatisfies(p => getValues(p).Any(collection.Contains));

    static Func<PalTribe, bool> AtLeastOneVariantSatisfies(Func<Pal, bool> predicate, bool expected = true) => tribe => tribe.Pals.Any(predicate) == expected;
}
