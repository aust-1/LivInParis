using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.Api.Controllers;

[ApiController]
[Route("api/chefs")]
public class ChefsController : ControllerBase
{
    private readonly IMenuProposalService _proposalSvc;
    private readonly IChefService _chefSvc;

    public ChefsController(IMenuProposalService proposalSvc, IChefService chefSvc)
    {
        _proposalSvc = proposalSvc;
        _chefSvc = chefSvc;
    }

    // GET: api/chefs/proposals
    [HttpGet("proposals")]
    public async Task<ActionResult<IEnumerable<MenuProposal>>> GetProposals() =>
        Ok(
            await _proposalSvc.GetByChefAsync( /* chefId from auth */
                1
            )
        );

    // POST: api/chefs/proposals
    [HttpPost("proposals")]
    public async Task<IActionResult> CreateProposal([FromBody] MenuProposal proposal)
    {
        await _proposalSvc.CreateAsync(proposal);
        return CreatedAtAction(nameof(GetProposals), null);
    }

    // PUT: api/chefs/proposals
    [HttpPut("proposals/{chefId}")]
    public async Task<IActionResult> UpdateProposal(int chefId, [FromBody] MenuProposal proposal)
    {
        var date = DateTime.Now;
        proposal.ChefAccountId = chefId;
        proposal.ProposalDate = DateOnly.FromDateTime(date);
        await _proposalSvc.UpdateAsync(proposal);
        return NoContent();
    }

    // DELETE: api/chefs/proposals
    [HttpDelete("proposals/{chefId}")]
    public async Task<IActionResult> DeleteProposal(int chefId)
    {
        await _proposalSvc.DeleteAsync(chefId, DateTime.Now);
        return NoContent();
    }

    // GET: api/chefs/orders/incoming
    [HttpGet("orders/incoming")]
    public async Task<ActionResult<IEnumerable<OrderLine>>> GetIncoming() =>
        Ok(
            await _chefSvc.GetIncomingOrdersAsync( /* chefId */
                1
            )
        );

    // POST: api/chefs/orders/{id}/accept
    [HttpPost("orders/{id}/accept")]
    public async Task<IActionResult> Accept(int id)
    {
        await _chefSvc.AcceptOrderAsync(id);
        return NoContent();
    }

    // POST: api/chefs/orders/{id}/reject
    [HttpPost("orders/{id}/reject")]
    public async Task<IActionResult> Reject(int id)
    {
        await _chefSvc.RejectOrderAsync(id);
        return NoContent();
    }

    // GET: api/chefs/deliveries
    [HttpGet("deliveries")]
    public async Task<ActionResult<IEnumerable<OrderLine>>> GetDeliveries() =>
        Ok(
            await _chefSvc.GetDeliveriesAsync( /* chefId */
                1
            )
        );

    // GET: api/chefs/deliveries/{id}
    [HttpGet("deliveries/{id}")]
    public async Task<ActionResult<OrderLine>> GetDelivery(int id)
    {
        var d = await _chefSvc.GetDeliveryDetailAsync(id);
        return d is null ? NotFound() : Ok(d);
    }
}
