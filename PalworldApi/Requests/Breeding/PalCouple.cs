using PalworldDataExtractor.Abstractions.Pals;

namespace PalworldApi.Requests.Breeding;

class PalCouple
{
    public required Pal PalA { get; set; }
    public required Pal PalB { get; set; }
}
