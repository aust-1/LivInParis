using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Individual")]
public class Individual
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string PersonalEmail { get; set; }

    [Required]
    [MaxLength(50)]
    public string PhoneNumber { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public Address Address { get; set; }

    [ForeignKey("AccountId")]
    public Customer Customer { get; set; }
}
