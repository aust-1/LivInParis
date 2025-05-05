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
    public int CustomerAccountId { get; set; }

    /// <summary>
    /// The rating of the customer.
    /// </summary>
    [Range(1.0, 5.0)]
    public decimal? CustomerRating { get; set; }

    /// <summary>
    /// Indicates whether the customer is banned from the platform.
    /// </summary>
    [Required]
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

TODO: delete rating

//TODO: check if a company already exists before creating an individual
