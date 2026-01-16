using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="AccountService"/>.
/// </summary>
public class AccountService(IAccountRepository accountRepository, ITokenService tokenService)
    : IAccountService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ITokenService _tokenService = tokenService;

    /// <inheritdoc/>
    public Task<Account?> GetAccountByIdAsync(int accountId) =>
        _accountRepository.GetByIdAsync(accountId);


    /// <inheritdoc/>
    public async Task<UpdateAccountResultDto> UpdateAccountAsync(UpdateAccountDto updateDto)
    {
        if (
            string.IsNullOrEmpty(updateDto.UserName)
            || string.IsNullOrEmpty(updateDto.NewPassword)
            || string.IsNullOrEmpty(updateDto.CurrentPassword)
        )
        {
            return new UpdateAccountResultDto
            {
                Success = false,
                Errors = ["Username, current password, and new password are required"],
            };
        }

        var account = await _accountRepository.FindByUserNameAsync(updateDto.UserName!);
        if (account == null || !_tokenService.VerifyPassword(updateDto.CurrentPassword!, account.AccountPassword))
        {
            return new UpdateAccountResultDto { Success = false, Errors = ["Invalid credentials"] };
        }

        account.AccountPassword = _tokenService.HashPassword(updateDto.NewPassword!);
        _accountRepository.Update(account);
        await _accountRepository.SaveChangesAsync();
        return new UpdateAccountResultDto { Success = true };
    }


    /// <inheritdoc/>
    public async Task DeleteAccountAsync(int accountId)
    {
        var account =
            await _accountRepository.GetByIdAsync(accountId)
            ?? throw new ArgumentException("Account not found", nameof(accountId));
        _accountRepository.Delete(account);
        await _accountRepository.SaveChangesAsync();
    }

}
