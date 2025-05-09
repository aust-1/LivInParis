using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

/// <summary>
/// Initializes a new instance of the <see cref="AuthController"/> class.
/// </summary>
/// <param name="authService">Service for authentication operations.</param>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// Authenticates a user and returns authentication tokens.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        if (!result.Success)
        {
            return Unauthorized(result.Errors);
        }
        return Ok(result);
    }

    /// <summary>
    /// Registers a new user and returns authentication tokens.
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResultDto>> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }

    /// <summary>
    /// Logs out the user by revoking their token.
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string token)
    {
        await _authService.LogoutAsync(token);
        return NoContent();
    }
}
