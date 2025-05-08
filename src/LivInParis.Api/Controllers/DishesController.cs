using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishesController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishesController(IDishService dishService) => _dishService = dishService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dish>>> Get()
    {
        var dishes = await _dishService.GetAllAsync();
        return Ok(dishes);
    }
}
