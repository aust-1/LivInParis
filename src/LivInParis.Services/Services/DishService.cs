using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Services.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

public class DishService : IDishService
{
    private readonly IDishRepository _dishRepo;

    public DishService(IDishRepository dishRepo) => _dishRepo = dishRepo;

    public Task<IEnumerable<Dish>> GetAllAsync() => _dishRepo.GetAllAsync();

    public Task<Dish?> GetByIdAsync(int dishId) => _dishRepo.GetByIdAsync(dishId);
}
