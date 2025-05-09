using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/chefs/{chefId}/profile")]
public class ChefProfileController(IChefProfileService profileService) : ControllerBase
{
    private readonly IChefProfileService _profileService = profileService;

    /// <summary>
    /// Gets a chef profile by chef id.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ChefProfileDto>> Get(int chefId)
    {
        var profile = await _profileService.GetProfileAsync(chefId);
        return Ok(profile);
    }

    /// <summary>
    /// Updates a chef profile.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update(int chefId, [FromBody] UpdateChefProfileDto dto)
    {
        await _profileService.UpdateProfileAsync(chefId, dto);
        return NoContent();
    }
}
