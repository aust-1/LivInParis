using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Ingredient")]
public class Ingredient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IngredientId { get; set; }

    [Required]
    [MaxLength(50)]
    public string IngredientName { get; set; }

    [Required]
    public bool IsVegetarian { get; set; }

    [Required]
    public bool IsVegan { get; set; }

    [Required]
    public bool IsGlutenFree { get; set; }

    [Required]
    public bool IsLactoseFree { get; set; }

    [Required]
    public bool IsHalal { get; set; }

    [Required]
    public bool IsKosher { get; set; }

    /// <summary>
    /// Relations vers les plats contenant cet ingrédient.
    /// </summary>
    public ICollection<Contains> Contains { get; set; }
}
