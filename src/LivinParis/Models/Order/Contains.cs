using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Contains")]
public class Contains
{
    public int IngredientId { get; set; }

    [ForeignKey("IngredientId")]
    public required Ingredient Ingredient { get; set; }

    public int DishId { get; set; }

    [ForeignKey("DishId")]
    public required Dish Dish { get; set; }
}

//TODO: add doc
