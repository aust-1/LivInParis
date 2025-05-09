using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GraphController(IGraphService graphService) : ControllerBase
{
    private readonly IGraphService _graphService = graphService;

    /// <summary>
    /// Gets shortest path between two addresses.
    /// </summary>
    [HttpGet("route")]
    public async Task<ActionResult<RouteDto>> GetRoute(
        [FromQuery] string fromAddress,
        [FromQuery] string toAddress
    )
    {
        var route = await _graphService.GetShortestPathAsync(fromAddress, toAddress);
        return Ok(route);
    }
}
