using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LivInParisRoussilleTeynier.Models.Order.Enums;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Dish")]
public class Dish
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DishId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string DishName { get; set; }

    [Required]
    public required DishType DishType { get; set; }

    [Required]
    public required int ExpiryTime { get; set; }

    [Required]
    [MaxLength(50)]
    public required string CuisineNationality { get; set; }

    //HACK: enum CuisineNationality

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive.")]
    public required int Quantity { get; set; }

    [Required]
    [Range(0, 10E9, ErrorMessage = "Price must be zero or positive.")]
    [Column(TypeName = "decimal(10,2)")]
    public required decimal Price { get; set; }

    [Required]
    public required ProductsOrigin ProductsOrigin { get; set; }

    [MaxLength(255)]
    public string? PhotoPath { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    public ICollection<MenuProposal> MenuProposals { get; set; } = new List<MenuProposal>();
    public ICollection<Contains> Contains { get; set; } = new List<Contains>();
}

//HACK: classe allergènes pour simplifier ingrédients + ajouter une ICollection dans Dish qui automatise la fusion de tous les ingrédients
//TODO: add doc
