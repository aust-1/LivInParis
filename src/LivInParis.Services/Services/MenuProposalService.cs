using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Services.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

public class MenuProposalService(IMenuProposalRepository repo) : IMenuProposalService
{
    private readonly IMenuProposalRepository _repo = repo;

    public async Task<IEnumerable<MenuProposal>> GetByChefAsync(int chefId)
    {
        return await _repo.ReadAsync(mp => mp.ChefAccountId == chefId);
    }

    public async Task CreateAsync(MenuProposal proposal)
    {
        await _repo.AddAsync(proposal);
        await _repo.SaveChangesAsync();
    }

    public async Task UpdateAsync(MenuProposal proposal)
    {
        _repo.Update(proposal);
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int chefId, DateTime proposalDate)
    {
        var existing = (
            await _repo.ReadAsync(mp =>
                mp.ChefAccountId == chefId && mp.ProposalDate == DateOnly.FromDateTime(proposalDate)
            )
        ).FirstOrDefault();
        if (existing != null)
        {
            _repo.Delete(existing);
            await _repo.SaveChangesAsync();
        }
    }
}
