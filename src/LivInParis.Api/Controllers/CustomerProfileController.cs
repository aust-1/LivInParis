using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/customers/{customerId}/profile")]
public class CustomerProfileController(ICustomerProfileService profileService) : ControllerBase
{
    private readonly ICustomerProfileService _profileService = profileService;

    /// <summary>
    /// Gets a customer profile by customer id.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<CustomerProfileDto>> Get(int customerId)
    {
        var profile = await _profileService.GetProfileAsync(customerId);
        return Ok(profile);
    }

    /// <summary>
    /// Updates a customer profile.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update(int customerId, [FromBody] UpdateCustomerProfileDto dto)
    {
        await _profileService.UpdateProfileAsync(customerId, dto);
        return NoContent();
    }
}
