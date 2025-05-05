using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

/// <summary>
/// Represents a company in the system.
/// </summary>
[Table("Company")]
[Index(nameof(CompanyName), IsUnique = true, Name = "IX_Company_CompanyName")]
public class Company
{
    /// <summary>
    /// The primary key for the company account.
    /// </summary>
    [Key]
    public int CompanyAccountId { get; set; }

    /// <summary>
    /// The name of the company.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string CompanyName { get; set; }

    /// <summary>
    /// The first name of the contact person for the company.
    /// </summary>
    [MaxLength(50)]
    public string? ContactFirstName { get; set; }

    /// <summary>
    /// The last name of the contact person for the company.
    /// </summary>
    [MaxLength(50)]
    public string? ContactLastName { get; set; }

    /// <summary>
    /// The account associated with the company.
    /// </summary>
    [ForeignKey("CompanyAccountId")]
    public Account? Account { get; set; }
}
