using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Individual")]
[Index(nameof(PhoneNumber), IsUnique = true, Name = "IX_Individual_PhoneNumber")]
public class Individual
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string PersonalEmail { get; set; }

    [Required]
    [MaxLength(50)]
    public required string PhoneNumber { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public required Address Address { get; set; }

    [ForeignKey("AccountId")]
    public required Customer Customer { get; set; }
}

//TODO: add docs
