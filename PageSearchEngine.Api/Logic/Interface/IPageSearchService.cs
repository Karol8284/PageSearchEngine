namespace PageSearchEngine.Api.Logic.Interface
{
    public interface IPageSearchService
    {
        Task<IEnumerable<PageSearchSuggestion>> SearchAsync(PageSearchRequest request, PageSearchResponse response, CancellationToken ct = default);
    }
}
