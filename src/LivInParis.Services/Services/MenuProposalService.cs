using LivInParisRoussilleTeynier.Domain.Models.Order;
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
    public async Task<IEnumerable<MenuProposalDto>> GetProposalsByChefAsync(int chefId)
    {
        var proposals = await _proposalRepository.GetProposalsByChefAsync(chefId);
        return proposals
            .GroupBy(p => p.ProposalDate)
            .Select(group => new MenuProposalDto
            {
                ChefId = chefId,
                ProposalDate = group.Key.ToDateTime(TimeOnly.MinValue),
                Dishes = group
                    .Where(p => p.Dish != null)
                    .Select(p => new DishDto
                    {
                        Id = p.Dish!.DishId,
                        Name = p.Dish.DishName,
                        Type = p.Dish.DishType.ToString(),
                        ExpiryTime = p.Dish.ExpiryTime,
                        Cuisine = p.Dish.CuisineNationality,
                        Quantity = p.Dish.Quantity,
                        Price = p.Dish.Price,
                        ProductsOrigin = p.Dish.ProductsOrigin.ToString(),
                        IsVegetarian = p.Dish.Contains.All(c => c.Ingredient!.IsVegetarian),
                        IsVegan = p.Dish.Contains.All(c => c.Ingredient!.IsVegan),
                        IsGlutenFree = p.Dish.Contains.All(c => c.Ingredient!.IsGlutenFree),
                        IsLactoseFree = p.Dish.Contains.All(c => c.Ingredient!.IsLactoseFree),
                        IsHalal = p.Dish.Contains.All(c => c.Ingredient!.IsHalal),
                        IsKosher = p.Dish.Contains.All(c => c.Ingredient!.IsKosher),
                        PhotoUrl = p.Dish.PhotoPath,
                    })
                    .ToList(),
            });
    }

    /// <inheritdoc/>
    public async Task CreateProposalAsync(CreateMenuProposalDto createDto)
    {
        var proposal = new MenuProposal
        {
            ChefAccountId = createDto.ChefId,
            ProposalDate = DateOnly.FromDateTime(createDto.ProposalDate),
            DishId = createDto.DishId,
        };

        await _proposalRepository.AddAsync(proposal);
        await _proposalRepository.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteProposalAsync(int chefId, DateTime proposalDate)
    {
        var date = DateOnly.FromDateTime(proposalDate);
        var proposals = await _proposalRepository.ReadAsync(p =>
            p.ChefAccountId == chefId && p.ProposalDate == date
        );

        foreach (var proposal in proposals)
        {
            _proposalRepository.Delete(proposal);
        }

        await _proposalRepository.SaveChangesAsync();
    }
}

