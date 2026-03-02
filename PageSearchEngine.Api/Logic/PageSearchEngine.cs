using Newtonsoft.Json;
using PageSearchEngine.Api.DTO;
using PageSearchEngine.Api.DTO.Data;
using PageSearchEngine.Api.DTO.Data.Object;
using PageSearchEngine.Api.Logic.Interface;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PageSearchEngine.Api.Logic
{
    public class PageSearchEngine
    {
        public PageSearchEngine() { }
        public Task<PageSearchResponse> Search(PageSearchRequest request)
        {
            var pagesDataObject = new PagesSuggestionData();
            PagesDataObject jsonPagesData = new PagesDataObject().ReturnDataFromPagesDataAsync();

            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.searchInPage)
            {

                foreach (var  in request.searchText)
                {
                    
                }
            }
            if (request.searchInDescription)
            {

            }
            if (request.searchInTitle)
            {

            }

            var answer = new PageSearchResponse();

            //silnik wyboru V1.0
            return Task <new PageSearchResponse()>; 
        }
    }
}
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json;
using PageSearchEngine.Api.DTO;
using PageSearchEngine.Api.Logic.Interface;

namespace PageSearchEngine.Api.Logic
{
    public class PageSearchEngine : IPageSearchService
    {
        private readonly string _dataFilePath;

        public PageSearchEngine(string dataFilePath)
        {
            _dataFilePath = dataFilePath ?? throw new ArgumentNullException(nameof(dataFilePath));
        }

        public async Task<PageSearchResponse> SearchAsync(PageSearchRequest request, CancellationToken ct = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ct.ThrowIfCancellationRequested();

            // Wczytaj plik asynchronicznie
            var json = await File.ReadAllTextAsync(_dataFilePath, ct);
            var container = JsonConvert.DeserializeObject<PagesContainer>(json)
                            ?? new PagesContainer { Pages = Array.Empty<PageRecord>() };

            // Normalizacja zapytania
            var query = Normalize(request.Query ?? string.Empty);

            // Proste filtrowanie i scoring (contains + optional JaroWinkler)
            var results = new List<PageSearchSuggestion>();
            foreach (var page in container.Pages)
            {
                ct.ThrowIfCancellationRequested();

                var title = Normalize(page.Title);
                var desc = Normalize(page.Description);
                var content = Normalize(page.Content);

                double score = 0.0;
                if (request.SearchInTitle && title.Contains(query)) score += 0.6;
                if (request.SearchInDescription && desc.Contains(query)) score += 0.25;
                if (request.SearchInPage && content.Contains(query)) score += 0.15;

                // jeśli nie ma prostego contains, policz podobieństwo (opcjonalnie)
                if (score == 0 && !string.IsNullOrEmpty(query))
                {
                    // prosty fallback: porównanie prefiksów / długości
                    if (title.StartsWith(query)) score = 0.5;
                }

                if (score > 0)
                {
                    results.Add(new PageSearchSuggestion
                    {
                        Title = page.Title,
                        Url = page.Path,
                        Score = Math.Round(score, 3)
                    });
                }
            }

            // sortuj po score i paginuj
            var ordered = results.OrderByDescending(r => r.Score).ToList();
            var total = ordered.Count;
            var items = ordered
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PageSearchResponse
            {
                Total = total,
                Items = items
            };
        }

        private static string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            s = s.Trim().ToLowerInvariant();
            s = RemoveDiacritics(s);
            s = Regex.Replace(s, @"\s+", " ");
            return s;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        // pomocniczy kontener do deserializacji JSON
        private class PagesContainer
        {
            public PageRecord[] Pages { get; set; } = Array.Empty<PageRecord>();
        }
    }
}