using Microsoft.AspNetCore.Mvc;
using PageSearchEngine.Api.DTO.Data.Object;
using PageSearchEngine.Api.Logic.Interface;

[ApiController]
[Route("api/pages")]
public class PageSearchEngineController : ControllerBase
{
    private readonly IPageSearchService _search;
    private readonly ILogger<PageSearchEngineController> _logger;

    public PageSearchEngineController(IPageSearchService search, ILogger<PageSearchEngineController> logger)
    {
        _search = search;
        _logger = logger;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PageSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PageSearchResponse>> Search([FromQuery] PageSearchRequest req, CancellationToken ct)
    {
        if (req == null) return BadRequest();
        try
        {
            var res = await _search.SearchAsync(req, ct);
            return Ok(res);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Search cancelled by client.");
            return StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Search failed.");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}