using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="AuthService"/>.
/// </summary>
public class AuthService(IAccountRepository userAccountRepository, ITokenService tokenService)
    : IAuthService
{
    private readonly IAccountRepository _userAccountRepository = userAccountRepository;
    private readonly ITokenService _tokenService = tokenService;

    /// <inheritdoc/>
    public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
    {
        if (string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = ["Username and password are required"],
            };
        }
        var user = await _userAccountRepository.ValidateCredentialsAsync(
            loginDto.Username,
            loginDto.Password
        );
        if (user == null)
        {
            return new AuthResultDto { Success = false, Errors = ["Invalid credentials"] };
        }

        var token = _tokenService.GenerateToken(user);
        return new AuthResultDto
        {
            Success = true,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            RefreshToken = _tokenService.GenerateRefreshToken(user),
        };
    }

    /// <inheritdoc/>
    public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
    {
        if (
            string.IsNullOrEmpty(registerDto.Username) || string.IsNullOrEmpty(registerDto.Password)
        )
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = ["Username and password are required"],
            };
        }
        var exists = await _userAccountRepository.ExistsAsync(registerDto.Username);
        if (exists)
        {
            return new AuthResultDto { Success = false, Errors = ["Username already taken"] };
        }

        var user = new Account
        {
            AccountUserName = registerDto.Username,
            AccountPassword = _tokenService.HashPassword(registerDto.Password),
        };

        await _userAccountRepository.AddAsync(user);
        await _userAccountRepository.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);
        return new AuthResultDto
        {
            Success = true,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            RefreshToken = _tokenService.GenerateRefreshToken(user),
        };
    }

    /// <inheritdoc/>
    public Task LogoutAsync(string token)
    {
        return _tokenService.RevokeTokenAsync(token);
    }
}
