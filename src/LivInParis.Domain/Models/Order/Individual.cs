using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

/// <summary>
/// Represents an individual in the system.
/// </summary>
[Table("Individual")]
[Index(nameof(PhoneNumber), IsUnique = true, Name = "IX_Individual_PhoneNumber")]
public class Individual
{
    /// <summary>
    /// The primary key for the individual account.
    /// </summary>
    [Key]
    public int IndividualAccountId { get; set; }

    /// <summary>
    /// The last name of the individual.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }

    /// <summary>
    /// The first name of the individual.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    /// <summary>
    /// The personal email address of the individual.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public required string PersonalEmail { get; set; }

    /// <summary>
    /// The phone number of the individual.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// The Id of the address associated with the individual.
    /// </summary>
    [Required]
    public int AddressId { get; set; }

    /// <summary>
    /// The account associated with the individual.
    /// </summary>
    [ForeignKey("IndividualAccountId")]
    public Account? Account { get; set; }

    /// <summary>
    /// The address associated with the individual.
    /// </summary>
    [ForeignKey("AddressId")]
    public Address? Address { get; set; }
}
