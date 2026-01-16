using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;


namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="CustomerProfileService"/>.
/// </summary>
public class CustomerProfileService(
    IAccountRepository accountRepository,
    ICustomerRepository customerRepository,
    IIndividualRepository individualRepository,
    ICompanyRepository companyRepository,
    IAddressRepository addressRepository
) : ICustomerProfileService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IIndividualRepository _individualRepository = individualRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IAddressRepository _addressRepository = addressRepository;

    /// <inheritdoc/>
    public async Task<CustomerProfileDto> GetProfileAsync(int customerId)
    {
        var account =
            await _accountRepository.GetByIdAsync(customerId)
            ?? throw new InvalidOperationException($"No account found with id {customerId}");
        var customer =
            await _customerRepository.GetByIdAsync(customerId)
            ?? throw new InvalidOperationException($"No customer found with id {customerId}");
        var individual = await _individualRepository.GetByIdAsync(account!.AccountId);
        if (individual != null)
        {
            var address = await _addressRepository.GetByIdAsync(individual.AddressId);
            return new CustomerProfileDto
            {
                CustomerId = customerId,
                Username = account.AccountUserName,
                IsBanned = customer.CustomerIsBanned,
                Rating = await _customerRepository.GetCustomerRatingAsync(customer),
                IsCompany = false,
                FirstName = individual.FirstName,
                LastName = individual.LastName,
                Email = individual.PersonalEmail,
                PhoneNumber = individual.PhoneNumber,
                Address = new AddressDto
                {
                    Id = address!.AddressId,
                    Number = address.AddressNumber,
                    Street = address.Street,
                },
            };
        }

        var company = await _companyRepository.GetByIdAsync(customerId);
        if (company != null)
        {
            return new CustomerProfileDto
            {
                CustomerId = customerId,
                IsCompany = true,
                CompanyName = company.CompanyName,
                ContactFirstName = company.ContactFirstName,
                ContactLastName = company.ContactLastName,
            };
        }

        throw new InvalidOperationException($"No customer found with id {customerId}");
    }

    /// <inheritdoc/>
    public async Task UpdateProfileAsync(int customerId, UpdateCustomerProfileDto updateDto)
    {
        if (updateDto.IsCompany)
        {
            _companyRepository.Update(
                new Company
                {
                    CompanyAccountId = customerId,
                    CompanyName = updateDto.CompanyName!,
                    ContactFirstName = updateDto.ContactFirstName,
                    ContactLastName = updateDto.ContactLastName,
                }
            );
            await _companyRepository.SaveChangesAsync();
            return;
        }

        if (updateDto.Address == null)
        {
            throw new ArgumentException("Address is required", nameof(updateDto));
        }

        var address = (
            await _addressRepository.ReadAsync(a =>
                a.Street == updateDto.Address.Street
                && a.AddressNumber == updateDto.Address.Number
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

        _individualRepository.Update(
            new Individual
            {
                IndividualAccountId = customerId,
                FirstName = updateDto.FirstName!,
                LastName = updateDto.LastName!,
                PersonalEmail = updateDto.Email!,
                PhoneNumber = updateDto.PhoneNumber!,
                Address = address,
                AddressId = address.AddressId,
            }
        );
        await _individualRepository.SaveChangesAsync();
    }
}
