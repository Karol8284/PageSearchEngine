using PageSearchEngine.Api.DTO.Data.Object;

namespace PageSearchEngine.Api.Logic.Interface
{
    public interface IPageSearchService
    {
        Task<PageSearchResponse> SearchAsync(PageSearchRequest request, CancellationToken ct = default);
    }
}
