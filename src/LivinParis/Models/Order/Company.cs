using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Company")]
[Index(nameof(CompanyName), IsUnique = true, Name = "IX_Company_CompanyName")]
public class Company : Customer
{
    [Required]
    [MaxLength(50)]
    public required string CompanyName { get; set; }

    [MaxLength(50)]
    public string? ContactFirstName { get; set; }

    [MaxLength(50)]
    public string? ContactLastName { get; set; }
}

//TODO: add doc
