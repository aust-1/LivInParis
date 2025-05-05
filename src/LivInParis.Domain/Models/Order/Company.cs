using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

[Table("Company")]
[Index(nameof(CompanyName), IsUnique = true, Name = "IX_Company_CompanyName")]
public class Company
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string CompanyName { get; set; }

    [MaxLength(50)]
    public string? ContactFirstName { get; set; }

    [MaxLength(50)]
    public string? ContactLastName { get; set; }

    [ForeignKey("AccountId")]
    public Account? Account { get; set; }
}

//TODO: add doc
