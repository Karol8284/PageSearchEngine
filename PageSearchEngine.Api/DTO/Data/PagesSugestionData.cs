using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PageSearchEngine.Api.DTO.Data.Object;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PageSearchEngine.Api.DTO.Data
{
    public class PagesSuggestionData
    {
        public List<PageSuggestionElement> Items { get; set; } = new List<PageSuggestionElement>();

        private readonly string _filePath;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "PagesDataJson";

        public PagesSuggestionData(IWebHostEnvironment env, IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            if (env == null) throw new ArgumentNullException(nameof(env));
            _filePath = Path.Combine(env.ContentRootPath, "StaticData", "PagesData.json");
        }

        public async Task<string> ReadRawJsonAsync(CancellationToken ct = default)
        {
            if (_cache.TryGetValue(CacheKey, out string cached)) return cached;

            ct.ThrowIfCancellationRequested();

            if (!File.Exists(_filePath))
                throw new FileNotFoundException("PagesData.json not found", _filePath);

            var json = await File.ReadAllTextAsync(_filePath, ct);

            _cache.Set(CacheKey, json, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) });

            return json;
        }

        /// <summary>
        /// Zwraca zdeserializowany obiekt PagesDataObject (lub null jeśli nie można zdeserializować).
        /// </summary>
        public async Task<PagesDataObject?> ReturnDataFromPagesDataAsync(CancellationToken ct = default)
        {
            if (_cache.TryGetValue(CacheKey, out string cached))
            {
                // jeśli mamy już JSON w cache, zdeserializuj go i zwróć
                return TryDeserialize(cached);
            }

            var json = await ReadRawJsonAsync(ct);
            return TryDeserialize(json);
        }

        private static PagesDataObject? TryDeserialize(string json)
        {
            try
            {
                // używamy Newtonsoft.Json bo masz go w projekcie
                var obj = JsonConvert.DeserializeObject<PagesDataObject>(json);
                return obj;
            }
            catch (JsonException)
            {
                // logowanie można dodać tutaj jeśli masz ILogger
                return null;
            }
        }

        public void InvalidateCache() => _cache.Remove(CacheKey);
    }
}