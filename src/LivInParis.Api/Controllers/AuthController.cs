using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Repositories;
using LivInParisRoussilleTeynier.Services;
using LivInParisRoussilleTeynier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest req)
    {
        var account = await _accountService.AuthenticateAsync(req.Name, req.Password);
        if (account is null)
            return Unauthorized();
        // TODO: generate JWT or session token
        return Ok(new { token = "REPLACE_WITH_TOKEN", role = "customer" });
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest req)
    {
        var account = await _accountService.GetAccountByUserNameAsync(req.Name);
        var individual = new Individual
        {
            Account = account,
            FirstName = req.Name,
            LastName = req.Name,
            PersonalEmail = req.Name,
            PhoneNumber = req.Name,
        };
        //FIXME: RegisterRequest should contain more information avec password in hashed format
        var result = await _accountService.RegisterIndividualAsync(individual, req.Password);
        return CreatedAtAction(nameof(GetProfile), new { id = result.IndividualAccountId }, result);
    }

    [HttpGet("profile")]
    public async Task<ActionResult<Account>> GetProfile()
    {
        // TODO: get accountId from token
        int accountId = 1;
        var account = await _accountService.GetByIdAsync(accountId);
        return account is Account ind ? Ok(ind) : NotFound();
    }

    [HttpPut("profile")]
    public ActionResult UpdateProfile([FromBody] Account update)
    {
        var updated = _accountService.Update(update);
        return NoContent();
    }
}
