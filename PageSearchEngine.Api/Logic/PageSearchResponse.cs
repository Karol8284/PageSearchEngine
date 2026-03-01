using PageSearchEngine.Api.Logic.Data;

namespace PageSearchEngine.Api.Logic
{
    public class PageSearchResponse
    {
        public Task<List<PageSuggestionElement>> pageResponse { get; set; }

        public PageSearchResponse(List<PageSuggestionElement> response)
        {
            pageResponse = Task.FromResult(response);
        }
    }
}
