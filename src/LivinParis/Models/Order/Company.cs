using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Company")]
public class Company
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CompanyName { get; set; }

    [MaxLength(50)]
    public string ContactFirstName { get; set; }

    [MaxLength(50)]
    public string ContactLastName { get; set; }

    [ForeignKey("AccountId")]
    public Customer Customer { get; set; }
}
