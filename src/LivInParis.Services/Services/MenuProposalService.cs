using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="MenuProposalService"/>.
/// </summary>
public class MenuProposalService(IMenuProposalRepository proposalRepository) : IMenuProposalService
{
    private readonly IMenuProposalRepository _proposalRepository = proposalRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<MenuProposalDto>> GetProposalsByChefAsync(int chefId) =>
        (IEnumerable<MenuProposalDto>)await _proposalRepository.GetProposalsByChefAsync(chefId);

    /// <inheritdoc/>
    public Task CreateProposalAsync(CreateMenuProposalDto createDto) =>
        _proposalRepository.AddAsync(
            new MenuProposal
            {
                ChefAccountId = createDto.ChefId,
                ProposalDate = DateOnly.FromDateTime(createDto.ProposalDate),
                DishId = createDto.DishId,
            }
        );

    /// <inheritdoc/>
    public Task DeleteProposalAsync(int chefId, DateTime proposalDate) =>
        throw new NotImplementedException("DeleteProposalAsync is not implemented.");
}
