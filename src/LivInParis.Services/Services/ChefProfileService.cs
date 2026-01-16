using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;


namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="ChefProfileService"/>.
/// </summary>
public class ChefProfileService(
    IAccountRepository accountRepository,
    IChefRepository chefRepository,
    IAddressRepository addressRepository
) : IChefProfileService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IChefRepository _chefRepository = chefRepository;
    private readonly IAddressRepository _addressRepository = addressRepository;

    /// <inheritdoc/>
    public async Task<ChefProfileDto> GetProfileAsync(int chefId)
    {
        var account =
            await _accountRepository.GetByIdAsync(chefId)
            ?? throw new InvalidOperationException($"No account found with id {chefId}");
        var chef =
            await _chefRepository.GetByIdAsync(chefId)
            ?? throw new InvalidOperationException($"No chef found with id {chefId}");
        var address = await _addressRepository.GetByIdAsync(chef.AddressId);

        return new ChefProfileDto
        {
            ChefId = chef.ChefAccountId,
            Username = account.AccountUserName,
            Rating = await _chefRepository.GetChefRatingAsync(chef),
            Address = new AddressDto
            {
                Id = address!.AddressId,
                Number = address!.AddressNumber,
                Street = address!.Street,
            },
        };
    }

    /// <inheritdoc/>
    public async Task UpdateProfileAsync(int chefId, UpdateChefProfileDto updateDto)
    {
        if (updateDto.Address == null)
        {
            throw new ArgumentException("Address is required", nameof(updateDto));
        }

        var address = (
            await _addressRepository.ReadAsync(a =>
                a.Street == updateDto.Address.Street && a.AddressNumber == updateDto.Address.Number
            )
        ).SingleOrDefault();

        if (address == null)
        {
            address = new Address
            {
                AddressNumber = updateDto.Address.Number,
                Street = updateDto.Address.Street!,
            };
            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();
        }

        _chefRepository.Update(
            new Chef
            {
                ChefAccountId = chefId,
                ChefIsBanned = updateDto.IsBanned,
                AddressId = address.AddressId,
            }
        );
        await _chefRepository.SaveChangesAsync();
    }

}
