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
    /// Searches dishes by criteria.
    /// </summary>
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<DishDto>>> Search(
        [FromBody] DishSearchCriteriaDto criteria
    )
    {
        var results = await _dishService.SearchDishesAsync(criteria);
        return Ok(results);
    }
}
