using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Contains")]
public class Contains
{
    public int IngredientId { get; set; }

    [ForeignKey("IngredientId")]
    public Ingredient Ingredient { get; set; }

    public int DishId { get; set; }

    [ForeignKey("DishId")]
    public Dish Dish { get; set; }
}
