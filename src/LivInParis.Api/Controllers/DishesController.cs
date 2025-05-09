using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishesController(IDishService dishService) : ControllerBase
{
    private readonly IDishService _dishService = dishService;

    /// <summary>
    /// Retrieves all dishes.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAll()
    {
        var dishes = await _dishService.GetAllDishesAsync();
        return Ok(dishes);
    }

    /// <summary>
    /// Retrieves a dish by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DishDto?>> GetById(int id)
    {
        var dish = await _dishService.GetDishByIdAsync(id);
        if (dish == null)
            return NotFound();
        return Ok(dish);
    }

    /// <summary>
    /// Searches dishes by criteria via query string.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> Search(
        [FromQuery] string? name,
        [FromQuery] string? type,
        [FromQuery] string? origin,
        [FromQuery] bool? vegetarian,
        [FromQuery] bool? vegan,
        [FromQuery] bool? glutenFree,
        [FromQuery] bool? lactoseFree,
        [FromQuery] bool? halal,
        [FromQuery] bool? kosher,
        [FromQuery] decimal? priceMin,
        [FromQuery] decimal? priceMax
    )
    {
        var criteria = new DishSearchCriteriaDto
        {
            Name = name,
            Type = type,
            ProductsOrigin = origin,
            IsVegetarian = vegetarian,
            IsVegan = vegan,
            IsGlutenFree = glutenFree,
            IsLactoseFree = lactoseFree,
            IsHalal = halal,
            IsKosher = kosher,
            MinPrice = priceMin,
            MaxPrice = priceMax,
        };

        var results = await _dishService.SearchDishesAsync(criteria);
        return Ok(results);
    }
}
