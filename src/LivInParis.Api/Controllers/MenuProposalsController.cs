using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/chefs/{chefId}/proposals")]
public class MenuProposalsController(IMenuProposalService proposalService) : ControllerBase
{
    private readonly IMenuProposalService _proposalService = proposalService;

    /// <summary>
    /// Retrieves all menu proposals for a chef.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuProposalDto>>> GetAll(int chefId)
    {
        var props = await _proposalService.GetProposalsByChefAsync(chefId);
        return Ok(props);
    }

    /// <summary>
    /// Creates a new menu proposal.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMenuProposalDto dto)
    {
        await _proposalService.CreateProposalAsync(dto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a menu proposal by date.
    /// </summary>
    [HttpDelete("{proposalDate}")]
    public async Task<IActionResult> Delete(int chefId, string proposalDate)
    {
        if (!DateTime.TryParse(proposalDate, out var date))
            return BadRequest("Invalid date format");
        await _proposalService.DeleteProposalAsync(chefId, date);
        return NoContent();
    }
}
