namespace LivInParisRoussilleTeynier.Services.Interfaces;

using LivInParisRoussilleTeynier.Domain.Models.Order;

public interface IDishService
{
    Task<IEnumerable<Dish>> GetAllAsync();
    Task<Dish?> GetByIdAsync(int dishId);
}
