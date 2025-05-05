using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

[Table("Individual")]
[Index(nameof(PhoneNumber), IsUnique = true, Name = "IX_Individual_PhoneNumber")]
public class Individual
{
    [Key]
    public int IndividualAccountId { get; set; }

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

    [Required]
    public int AddressId { get; set; }

    [ForeignKey("IndividualAccountId")]
    public Account? Account { get; set; }

    [ForeignKey("AddressId")]
    public Address? Address { get; set; }
}

//TODO: add docs
