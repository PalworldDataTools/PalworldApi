using PalworldApi.Models;
using SkiaSharp;

namespace PalworldApi.Services;

/// <summary>
///     Get Pal icons at different sizes
/// </summary>
public class PalIconsService
{
    readonly RawDataService _rawDataService;
    readonly Dictionary<string, Dictionary<string, MultiSizeIcon>> _cache = new();

    /// <summary>
    ///     Create the Pal icons service
    /// </summary>
    public PalIconsService(RawDataService rawDataService)
    {
        _rawDataService = rawDataService;
    }

    /// <summary>
    ///     Get the icon of the corresponding pal tribe at the requested size. If the size is omitted, the original icon is used.
    /// </summary>
    public async Task<byte[]?> GetPalIconAsync(string tribeName, (int Width, int Height)? size = null, string? version = null)
    {
        version ??= RawDataService.DefaultVersion;
        if (!_cache.TryGetValue(version, out Dictionary<string, MultiSizeIcon>? palIcons) || !palIcons.TryGetValue(tribeName, out MultiSizeIcon? multiSizeIcon))
        {
            VersionedData? data = await _rawDataService.GetData(version);
            if (data == null)
            {
                return null;
            }


            if (!data.Data.TribeIcons.TryGetValue(tribeName, out byte[]? icon))
            {
                return null;
            }

            multiSizeIcon = new MultiSizeIcon(icon);

            _cache.TryAdd(version, new Dictionary<string, MultiSizeIcon>());
            _cache[version][tribeName] = multiSizeIcon;
        }

        return multiSizeIcon.Get(size);
    }

    class MultiSizeIcon
    {
        readonly SKBitmap _original;
        readonly Dictionary<(int, int), byte[]> _imagesCache;

        public MultiSizeIcon(byte[] original)
        {
            _original = SKBitmap.Decode(original);
            _imagesCache = new Dictionary<(int, int), byte[]> { { (_original.Width, _original.Height), original } };
        }

        public byte[] Get((int Width, int Height)? size = null)
        {
            size ??= (_original.Width, _original.Height);

            if (_imagesCache.TryGetValue(size.Value, out byte[]? icon))
            {
                return icon;
            }

            SKBitmap resized = new(size.Value.Width, size.Value.Height, _original.ColorType, _original.AlphaType);
            if (!_original.ScalePixels(resized, SKFilterQuality.High))
            {
                throw new Exception("Could not resize image");
            }

            SKData? resizedIconPng = resized.Encode(SKEncodedImageFormat.Png, 90);
            if (resized == null)
            {
                throw new Exception("Could not encode resized image");
            }


            byte[] resizedIcon = resizedIconPng.ToArray();
            _imagesCache[size.Value] = resizedIcon;

            return resizedIcon;
        }
    }
}
