using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using PalworldApi.Models;
using PalworldApi.Rest.v1.Models.Pals;
using PalworldDataExtractor.Abstractions.Breeding;
using Pal = PalworldDataExtractor.Abstractions.Pals.Pal;

namespace PalworldApi.Requests.Breeding;

class UnbreedPals : IRequestHandler<UnbreedPalsRequest, IReadOnlyCollection<PalCouple>>
{
    static readonly ParentsCache Cache = new();
    static ImmutableSortedSet<Pal>? _palSortedByCombiRank;

    readonly IMediator _mediator;

    public UnbreedPals(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IReadOnlyCollection<PalCouple>> Handle(UnbreedPalsRequest request, CancellationToken cancellationToken)
    {
        if (Cache.TryFind(request.Data.Version, request.Pal.Name, out IReadOnlyCollection<PalCouple>? parents))
        {
            return parents;
        }

        IReadOnlyCollection<PalCouple> result;

        IEnumerable<PalCouple>? parentsFromUniqueCombinations = ComputeParentsFromUniqueCombinations(request.Data, request.Pal);
        if (parentsFromUniqueCombinations != null)
        {
            result = parentsFromUniqueCombinations.ToArray();
        }
        else
        {
            result = (await ComputeParentsFromNormalCombinations(request.Data, request.Pal)).ToArray();
        }

        Cache.Add(request.Data.Version, request.Pal.Name, result);

        return result;
    }

    static IEnumerable<PalCouple>? ComputeParentsFromUniqueCombinations(VersionedData data, Pal pal)
    {
        PalBreedingCombination[] combinations = data.Data.UniqueBreedingCombinations.Where(c => c.ChildCharacterId == pal.Name).ToArray();
        if (combinations.Length == 0)
        {
            return null;
        }

        return combinations.Select(c => new PalCouple { PalA = RequirePalFromTribeName(data, c.ParentTribeA), PalB = RequirePalFromTribeName(data, c.ParentTribeB) });
    }

    async Task<IEnumerable<PalCouple>> ComputeParentsFromNormalCombinations(VersionedData data, Pal pal)
    {
        ImmutableSortedSet<Pal> sortedPals = GetPalsSortedByCombiRank(data);
        int index = sortedPals.IndexOf(pal);

        List<PalCouple> parents = new();

        for (int parentAIndex = 0; parentAIndex <= index; parentAIndex++)
        for (int parentBIndex = index; parentBIndex < sortedPals.Count; parentBIndex++)
        {
            Pal palA = sortedPals[parentAIndex];
            Pal palB = sortedPals[parentBIndex];

            Pal child = await _mediator.Send(new BreedPalsRequest { Data = data, PalA = palA, PalB = palB, IncludeUniqueCombinations = false });
            if (child.Name == pal.Name)
            {
                parents.Add(new PalCouple { PalA = palA, PalB = palB });
            }
        }

        return parents;
    }

    static ImmutableSortedSet<Pal> GetPalsSortedByCombiRank(VersionedData data) =>
        _palSortedByCombiRank ??= data.Data.Tribes.SelectMany(t => t.Pals)
            .Where(p => p is { CombiRank: not 0, IsBoss: false, IsTowerBoss: false })
            .ToImmutableSortedSet(new CombiRankComparer());

    static Pal RequirePalFromTribeName(VersionedData data, string tribeName) =>
        data.Data.Tribes.FirstOrDefault(t => t.Name == tribeName)?.GetMainPal() ?? throw new Exception($"Could not find main pal of tribe {tribeName}");

    class CombiRankComparer : IComparer<Pal>
    {
        public int Compare(Pal? x, Pal? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }
            if (ReferenceEquals(null, y))
            {
                return 1;
            }
            if (ReferenceEquals(null, x))
            {
                return -1;
            }
            return x.CombiRank.CompareTo(y.CombiRank);
        }
    }

    class ParentsCache
    {
        readonly Dictionary<string, List<ParentsCacheEntry>> _parentsCache = new();

        public bool TryFind(string version, string pal, [NotNullWhen(true)] out IReadOnlyCollection<PalCouple>? parents)
        {
            if (!_parentsCache.TryGetValue(version, out List<ParentsCacheEntry>? cachedValues))
            {
                parents = null;
                return false;
            }

            parents = cachedValues.SingleOrDefault(e => e.Pal == pal)?.Parents;
            return parents != null;
        }

        public void Add(string version, string pal, IReadOnlyCollection<PalCouple> parents)
        {
            if (!_parentsCache.TryGetValue(version, out List<ParentsCacheEntry>? entries))
            {
                entries = new List<ParentsCacheEntry>();
                _parentsCache[version] = entries;
            }

            entries.Add(new ParentsCacheEntry { Pal = pal, Parents = parents });
        }
    }

    class ParentsCacheEntry
    {
        public required string Pal { get; init; }
        public required IReadOnlyCollection<PalCouple> Parents { get; init; }
    }
}

class UnbreedPalsRequest : IRequest<IReadOnlyCollection<PalCouple>>
{
    public required VersionedData Data { get; init; }
    public required Pal Pal { get; init; }
}
