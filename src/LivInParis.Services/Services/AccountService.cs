using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="AccountService"/>.
/// </summary>
public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    private readonly IAccountRepository _accountRepository = accountRepository;

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

        if (
            _accountRepository.ValidateCredentialsAsync(
                updateDto.UserName!,
                updateDto.CurrentPassword!
            ) == null
        )
        {
            return new UpdateAccountResultDto { Success = false, Errors = ["Invalid credentials"] };
        }

        var id =
            (await _accountRepository.FindByUserNameAsync(updateDto.UserName!))?.AccountId!
            ?? throw new ArgumentException("Account not found");
        _accountRepository.Update(
            new Account
            {
                AccountId = id,
                AccountUserName = updateDto.UserName!,
                AccountPassword = updateDto.NewPassword!,
            }
        );
        return new UpdateAccountResultDto { Success = true };
    }

    /// <inheritdoc/>
    public async Task DeleteAccountAsync(int accountId)
    {
        var account =
            await _accountRepository.GetByIdAsync(accountId)
            ?? throw new ArgumentException("Account not found", nameof(accountId));
        _accountRepository.Delete(account);
    }
}
