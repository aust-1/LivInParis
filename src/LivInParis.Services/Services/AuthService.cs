using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="AuthService"/>.
/// </summary>
public class AuthService(
    IAccountRepository userAccountRepository,
    ITokenService tokenService,
    ICustomerRepository customerRepository,
    IIndividualRepository individualRepository,
    ICompanyRepository companyRepository,
    IAddressRepository addressRepository
) : IAuthService
{
    private readonly IAccountRepository _userAccountRepository = userAccountRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IIndividualRepository _individualRepository = individualRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IAddressRepository _addressRepository = addressRepository;

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
        var user = await _userAccountRepository.FindByUserNameAsync(loginDto.Username);
        if (user == null || !_tokenService.VerifyPassword(loginDto.Password, user.AccountPassword))
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
            AccountId = user.AccountId,
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

        if (registerDto.IsCompany)
        {
            if (string.IsNullOrWhiteSpace(registerDto.CompanyName))
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = ["Company name is required"],
                };
            }
        }
        else
        {
            if (
                string.IsNullOrWhiteSpace(registerDto.Email)
                || string.IsNullOrWhiteSpace(registerDto.FirstName)
                || string.IsNullOrWhiteSpace(registerDto.LastName)
                || string.IsNullOrWhiteSpace(registerDto.PhoneNumber)
                || string.IsNullOrWhiteSpace(registerDto.Street)
                || registerDto.AddressNumber == null
            )
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = ["Missing required profile fields"],
                };
            }
        }

        var user = new Account
        {
            AccountUserName = registerDto.Username,
            AccountPassword = _tokenService.HashPassword(registerDto.Password),
        };

        await _userAccountRepository.AddAsync(user);
        await _userAccountRepository.SaveChangesAsync();

        await _customerRepository.AddAsync(
            new Customer
            {
                CustomerAccountId = user.AccountId,
                CustomerIsBanned = false,
            }
        );

        if (registerDto.IsCompany)
        {
            await _companyRepository.AddAsync(
                new Company
                {
                    CompanyAccountId = user.AccountId,
                    CompanyName = registerDto.CompanyName!,
                    ContactFirstName = registerDto.ContactFirstName,
                    ContactLastName = registerDto.ContactLastName,
                }
            );
        }
        else
        {
            var address = (
                await _addressRepository.ReadAsync(a =>
                    a.AddressNumber == registerDto.AddressNumber
                    && a.Street == registerDto.Street
                )
            ).SingleOrDefault();
            if (address == null)
            {
                address = new Address
                {
                    AddressNumber = registerDto.AddressNumber!.Value,
                    Street = registerDto.Street!,
                };
                await _addressRepository.AddAsync(address);
            }

            await _individualRepository.AddAsync(
                new Individual
                {
                    IndividualAccountId = user.AccountId,
                    FirstName = registerDto.FirstName!,
                    LastName = registerDto.LastName!,
                    PersonalEmail = registerDto.Email!,
                    PhoneNumber = registerDto.PhoneNumber!,
                    Address = address,
                }
            );
        }

        await _customerRepository.SaveChangesAsync();
        await _companyRepository.SaveChangesAsync();
        await _individualRepository.SaveChangesAsync();
        await _addressRepository.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);
        return new AuthResultDto
        {
            Success = true,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            RefreshToken = _tokenService.GenerateRefreshToken(user),
            AccountId = user.AccountId,
        };
    }


    /// <inheritdoc/>
    public Task LogoutAsync(string token)
    {
        return _tokenService.RevokeTokenAsync(token);
    }
}
