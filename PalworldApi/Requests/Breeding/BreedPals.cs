using System.Diagnostics.CodeAnalysis;
using MediatR;
using PalworldApi.Models;
using PalworldDataExtractor.Abstractions.Pals;

namespace PalworldApi.Requests.Breeding;

class BreedPals : IRequestHandler<BreedPalsRequest, Pal>
{
    static readonly CombinationCache Cache = new();
    static readonly Dictionary<string, IReadOnlyCollection<Pal>> PalsWithoutUniqueCombination = new();

    public Task<Pal> Handle(BreedPalsRequest request, CancellationToken cancellationToken)
    {
        if (Cache.TryFind(request.Data.Version, request.PalA.Name, request.PalB.Name, out Pal? cachedChild))
        {
            return Task.FromResult(cachedChild);
        }

        if (request is { IncludeUniqueCombinations: true, PalA.TribeName: not null, PalB.TribeName: not null }
            && TryFindUniqueCombination(request.Data, request.PalA.TribeName, request.PalB.TribeName, out string? childName))
        {
            if (!TryFindPal(request.Data, childName, out Pal? childPal))
            {
                throw new Exception($"Could not find pal with name {childName}");
            }

            return Task.FromResult(childPal);
        }

        int combiRankA = request.PalA.CombiRank;
        int combiRankB = request.PalB.CombiRank;
        double mean = (float)(combiRankA + combiRankB) / 2;
        Pal[] closestPals = GetPalsWithoutUniqueCombination(request.Data)
            .Select(p => new { Pal = p, Rank = Math.Abs(p.CombiRank - mean) })
            .GroupBy(x => x.Rank)
            .OrderBy(g => g.Key)
            .Select(g => g.Select(x => x.Pal))
            .First()
            .ToArray();

        // If multiple pals are tied, select the one with the smallest Zukan index
        Pal child = closestPals.Length switch
        {
            1 => closestPals[0],
            _ => closestPals.OrderBy(p => p.ZukanIndex).First()
        };

        Cache.Add(request.Data.Version, request.PalA.Name, request.PalB.Name, child);

        return Task.FromResult(child);
    }

    static IReadOnlyCollection<Pal> GetPalsWithoutUniqueCombination(VersionedData data)
    {
        if (!PalsWithoutUniqueCombination.TryGetValue(data.Version, out IReadOnlyCollection<Pal>? result))
        {
            result = data.Data.Tribes.SelectMany(t => t.Pals)
                .Where(p => p is { CombiRank: not 0, IsBoss: false, IsTowerBoss: false })
                .Where(p => data.Data.UniqueBreedingCombinations.All(c => c.ChildCharacterId != p.Name))
                .ToArray();
            PalsWithoutUniqueCombination[data.Version] = result;
        }

        return result;
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

        public bool TryFind(string version, string palA, string palB, [NotNullWhen(true)] out Pal? pal)
        {
            if (!_combinationsCache.TryGetValue(version, out List<CombinationCacheEntry>? cachedValues))
            {
                pal = null;
                return false;
            }

            pal = cachedValues.SingleOrDefault(e => e.PalA == palA && e.PalB == palB)?.Child;
            return pal != null;
        }

        public void Add(string version, string palA, string palB, Pal child)
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
    public bool IncludeUniqueCombinations { get; init; } = true;
}
