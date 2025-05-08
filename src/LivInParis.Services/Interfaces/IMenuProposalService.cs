namespace LivInParisRoussilleTeynier.Services.Interfaces;

using LivInParisRoussilleTeynier.Domain.Models.Order;

public interface IMenuProposalService
{
    Task<IEnumerable<MenuProposal>> GetByChefAsync(int chefId);
    Task CreateAsync(MenuProposal proposal);
    Task UpdateAsync(MenuProposal proposal);
    Task DeleteAsync(int chefId, DateTime proposalDate);
}
