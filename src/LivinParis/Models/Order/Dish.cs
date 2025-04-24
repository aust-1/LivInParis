using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Dish")]
public class Dish
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DishId { get; set; }

    [Required]
    [MaxLength(50)]
    public string DishName { get; set; }

    [Required]
    public DishType DishType { get; set; }

    [Required]
    public int ExpiryTime { get; set; }

    [Required]
    [MaxLength(50)]
    public string CuisineNationality { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    public ProductOrigin ProductOrigin { get; set; }

    [MaxLength(255)]
    public string PhotoPath { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
    public ICollection<MenuProposal> MenuProposals { get; set; }
    public ICollection<Contains> Contains { get; set; }
}
