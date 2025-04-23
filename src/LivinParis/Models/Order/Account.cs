using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Account")]
public class Account
{
    /// <summary>
    /// Primary key, auto-incremented by the database.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AccountId { get; set; }

    /// <summary>
    /// Unique email address of the account.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string AccountEmail { get; set; }

    /// <summary>
    /// Password of the account.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string AccountPassword { get; set; }

    /// <summary>
    /// Customer profile associated with the account (if any).
    /// </summary>
    public Customer? Customer { get; set; }

    /// <summary>
    /// Chef profile associated with the account (if any).
    /// </summary>
    public Chef? Chef { get; set; }

    /// <summary>
    /// Company profile associated with the account (if any).
    /// </summary>
    public Company? Company { get; set; }

    /// <summary>
    /// Individual profile associated with the account (if any).
    /// </summary>
    public Individual? Individual { get; set; }
}
