using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

/// <summary>
/// Represents a customer in the system.
/// </summary>
[Table("Customer")]
public class Customer
{
    /// <summary>
    /// The primary key for the customer account.
    /// </summary>
    [Key]
    [Column("account_id")]
    public int CustomerAccountId { get; set; }

    /// <summary>
    /// Indicates whether the customer is banned from the platform.
    /// </summary>
    [Required]
    [Column("customer_is_banned")]
    public bool CustomerIsBanned { get; set; }

    /// <summary>
    /// The account associated with the customer.
    /// </summary>
    [ForeignKey("CustomerAccountId")]
    public Account? Account { get; set; }

    /// <summary>
    /// The transactions associated with the customer.
    /// </summary>
    public ICollection<OrderTransaction> OrderTransactions { get; set; } =
        new List<OrderTransaction>();
}
