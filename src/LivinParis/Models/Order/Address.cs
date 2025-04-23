using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Address")]
public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AddressId { get; set; }

    [Required]
    public int AddressNumber { get; set; }

    [Required]
    [MaxLength(50)]
    public string Street { get; set; }

    [MaxLength(50)]
    public string NearestMetro { get; set; }

    /// <summary>
    /// Chefs résidant à cette adresse.
    /// </summary>
    public ICollection<Chef> Chefs { get; set; }

    /// <summary>
    /// Particuliers domiciliés à cette adresse.
    /// </summary>
    public ICollection<Individual> Individuals { get; set; }

    /// <summary>
    /// Lignes de commande pour livraison à cette adresse.
    /// </summary>
    public ICollection<OrderLine> OrderLines { get; set; }
}
