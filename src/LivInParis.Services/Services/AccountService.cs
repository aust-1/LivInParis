using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Services.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly IIndividualRepository _individualRepo;
    private readonly IChefRepository _chefRepo;
    private readonly ICompanyRepository _companyRepo;

    public AccountService(
        IAccountRepository accountRepo,
        ICustomerRepository customerRepo,
        IIndividualRepository individualRepo,
        IChefRepository chefRepo,
        ICompanyRepository companyRepo
    )
    {
        _accountRepo = accountRepo;
        _customerRepo = customerRepo;
        _individualRepo = individualRepo;
        _chefRepo = chefRepo;
        _companyRepo = companyRepo;
    }

    public async Task<Account?> AuthenticateAsync(string userName, string password)
    {
        var account = await _accountRepo.FindByUserNameAsync(userName);
        if (account == null || account.AccountPassword != password)
            return null;
        return account;
    }

    public async Task<Individual> RegisterIndividualAsync(Individual individual, string password)
    {
        // create account
        var acc = new Account { AccountUserName = individual.UserName, AccountPassword = password };
        await _accountRepo.AddAsync(acc);
        // create customer (shared base)
        var cust = new Customer
        {
            CustomerAccountId = acc.AccountId,
            CustomerIsBanned = false,
            CustomerRating = 0,
        };
        await _customerRepo.AddAsync(cust);
        // create individual record
        individual.IndividualAccountId = acc.AccountId;
        await _individualRepo.AddAsync(individual);
        return individual;
    }

    public async Task<Company> RegisterCompanyAsync(Company company, string password)
    {
        var acc = new Account { AccountUserName = company.CompanyName, AccountPassword = password };
        await _accountRepo.AddAsync(acc);
        var cust = new Customer
        {
            CustomerAccountId = acc.AccountId,
            CustomerIsBanned = false,
            CustomerRating = 0,
        };
        await _customerRepo.AddAsync(cust);
        company.CompanyAccountId = acc.AccountId;
        await _companyRepo.AddAsync(company);
        return company;
    }

    public async Task<Account?> GetAccountByUserNameAsync(string userName)
    {
        return await _accountRepo.FindByUserNameAsync(userName);
    }

    public async Task<Account?> GetByIdAsync(int accountId)
    {
        return await _accountRepo.GetByIdAsync(accountId);
    }

    public Account Update(Account account)
    {
        _accountRepo.Update(account);
        return account;
    }

    public void Delete(int accountId)
    {
        var account = _accountRepo.GetByIdAsync(accountId).Result;
        if (account != null)
        {
            _accountRepo.Delete(account);
        }
    }
}
