using Microsoft.AspNetCore.Mvc;
using PageSearchEngine.Api.Logic;
using PageSearchEngine.Api.Logic.Interface;

namespace PageSearchEngine.Api.Controllers
{
    [ApiController]
    [Route("api/pages")]
    public class PageSearchEngineController : ControllerBase
    {
        private readonly IPageSearchService _search;
        public PageSearchEngineController(IPageSearchService search) => _search = search;
        public PageSearchEngine.Api.Logic.PageSearchEngine pageSearchEngine = new();


        [HttpGet("search")]
        //public async Task<IActionResult> Search([FromQuery] PageSearchRequest req, CancellationToken ct)
        public async Task<PageSearchResponse> Search([FromQuery] PageSearchRequest req, CancellationToken ct)
        {
            var res = await pageSearchEngine.Search(req);
            //var res = await _search.SearchAsync(req, ct);
            
            return Ok(res);
        }

    }
}
