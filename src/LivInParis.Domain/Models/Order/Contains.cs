using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

/// <summary>
/// Represents a relationship between an ingredient and a dish in the system.
/// </summary>
[Table("Contains")]
public class Contains
{
    /// <summary>
    /// One of the primary keys for the Contains table.
    /// The ingredient associated with this dish.
    /// </summary>
    [Required]
    [Column("ingredient_id")]
    public required int IngredientId { get; set; }

    /// <summary>
    /// One of the primary keys for the Contains table.
    /// The dish associated with this ingredient.
    /// </summary>
    [Required]
    [Column("dish_id")]
    public required int DishId { get; set; }

    /// <summary>
    /// The ingredient associated with this dish.
    /// </summary>
    [ForeignKey("IngredientId")]
    public Ingredient? Ingredient { get; set; }

    /// <summary>
    /// The dish associated with this ingredient.
    /// </summary>
    [ForeignKey("DishId")]
    public Dish? Dish { get; set; }
}
