using System.Diagnostics.CodeAnalysis;
using MediatR;
using PalworldDataExtractor.Abstractions;
using PalworldDataExtractor.Abstractions.Pals;

namespace PalworldApi.Requests.Breeding;

class BreedPals : IRequestHandler<BreedPalsRequest, Pal>
{
    public Task<Pal> Handle(BreedPalsRequest request, CancellationToken cancellationToken)
    {
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
        Pal closestPal = request.Data.Tribes.SelectMany(t => t.Pals).Where(p => p is { IsBoss: false, IsTowerBoss: false }).OrderBy(p => Math.Abs(p.CombiRank - mean)).First();

        return Task.FromResult(closestPal);
    }

    static bool TryFindUniqueCombination(ExtractedData data, string tribeNameA, string tribeNameB, [NotNullWhen(true)] out string? result)
    {
        result = data.UniqueBreedingCombinations.FirstOrDefault(c => c.ParentTribeA == tribeNameA && c.ParentTribeB == tribeNameB)?.ChildCharacterId;
        return result != null;
    }

    static bool TryFindPal(ExtractedData data, string palName, [NotNullWhen(true)] out Pal? pal)
    {
        pal = data.Tribes.SelectMany(t => t.Pals).FirstOrDefault(t => t.Name == palName);
        return pal != null;
    }
}

class BreedPalsRequest : IRequest<Pal>
{
    public required ExtractedData Data { get; init; }
    public required Pal PalA { get; init; }
    public required Pal PalB { get; init; }
}
