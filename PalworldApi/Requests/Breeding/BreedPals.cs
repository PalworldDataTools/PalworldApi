using System.Diagnostics.CodeAnalysis;
using MediatR;
using PalworldApi.Models;
using PalworldDataExtractor.Abstractions.Pals;

namespace PalworldApi.Requests.Breeding;

class BreedPals : IRequestHandler<BreedPalsRequest, Pal>
{
    static readonly CombinationCache Cache = new();

    public Task<Pal> Handle(BreedPalsRequest request, CancellationToken cancellationToken)
    {
        if (Cache.TryFindCachedValue(request.Data.Version, request.PalA.Name, request.PalB.Name, out Pal? cachedChild))
        {
            return Task.FromResult(cachedChild);
        }

        if (request.PalA.TribeName != null
            && request.PalB.TribeName != null
            && TryFindUniqueCombination(request.Data, request.PalA.TribeName, request.PalB.TribeName, out string? child))
        {
            if (!TryFindPal(request.Data, child, out Pal? childPal))
            {
                throw new Exception($"Could not find pal with name {child}");
            }

            return Task.FromResult(childPal);
        }

        int combiRankA = request.PalA.CombiRank;
        int combiRankB = request.PalB.CombiRank;
        int mean = (int)Math.Floor((float)(combiRankA + combiRankB + 1) / 2);
        Pal closestPal = request.Data.Data.Tribes.SelectMany(t => t.Pals).Where(p => p is { IsBoss: false, IsTowerBoss: false }).OrderBy(p => Math.Abs(p.CombiRank - mean)).First();

        Cache.CacheResult(request.Data.Version, request.PalA.Name, request.PalB.Name, closestPal);

        return Task.FromResult(closestPal);
    }

    static bool TryFindUniqueCombination(VersionedData data, string tribeNameA, string tribeNameB, [NotNullWhen(true)] out string? result)
    {
        result = data.Data.UniqueBreedingCombinations.FirstOrDefault(c => c.ParentTribeA == tribeNameA && c.ParentTribeB == tribeNameB)?.ChildCharacterId;
        return result != null;
    }

    static bool TryFindPal(VersionedData data, string palName, [NotNullWhen(true)] out Pal? pal)
    {
        pal = data.Data.Tribes.SelectMany(t => t.Pals).FirstOrDefault(t => t.Name == palName);
        return pal != null;
    }


    class CombinationCache
    {
        readonly Dictionary<string, List<CombinationCacheEntry>> _combinationsCache = new();

        public bool TryFindCachedValue(string version, string palA, string palB, [NotNullWhen(true)] out Pal? pal)
        {
            if (!_combinationsCache.TryGetValue(version, out List<CombinationCacheEntry>? cachedValues))
            {
                pal = null;
                return false;
            }

            pal = cachedValues.SingleOrDefault(e => e.PalA == palA && e.PalB == palB)?.Child;
            return pal != null;
        }

        public void CacheResult(string version, string palA, string palB, Pal child)
        {
            if (!_combinationsCache.TryGetValue(version, out List<CombinationCacheEntry>? entries))
            {
                entries = new List<CombinationCacheEntry>();
                _combinationsCache[version] = entries;
            }

            entries.Add(new CombinationCacheEntry { PalA = palA, PalB = palB, Child = child });
        }
    }

    class CombinationCacheEntry
    {
        public required string PalA { get; init; }
        public required string PalB { get; init; }
        public required Pal Child { get; init; }
    }
}

class BreedPalsRequest : IRequest<Pal>
{
    public required VersionedData Data { get; init; }
    public required Pal PalA { get; init; }
    public required Pal PalB { get; init; }
}
