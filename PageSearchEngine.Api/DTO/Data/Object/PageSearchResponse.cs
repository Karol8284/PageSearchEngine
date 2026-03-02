using PageSearchEngine.Api.DTO.Data;

namespace PageSearchEngine.Api.DTO.Data.Object
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
