using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;

    /// <summary>
    /// Gets account information by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto?>> GetById(int id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null)
            return NotFound();

        var dto = new AccountDto
        {
            Id = account.AccountId,
            Username = account.AccountUserName,
            Email = null,
        };
        return Ok(dto);
    }


    /// <summary>
    /// Updates account details.
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<UpdateAccountResultDto>> Update([FromBody] UpdateAccountDto dto)
    {
        var result = await _accountService.UpdateAccountAsync(dto);
        if (!result.Success)
            return BadRequest(result.Errors);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an account by id.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _accountService.DeleteAccountAsync(id);
        return NoContent();
    }
}
