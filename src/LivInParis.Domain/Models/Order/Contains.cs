using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

[Table("Contains")]
public class Contains
{
    [Required]
    public required int IngredientId { get; set; }

    [Required]
    public required int DishId { get; set; }

    [ForeignKey("IngredientId")]
    public Ingredient? Ingredient { get; set; }

    [ForeignKey("DishId")]
    public Dish? Dish { get; set; }
}

//TODO: add doc
