using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Customer")]
public class Customer
{
    [Key]
    public int AccountId { get; set; }

    /// <summary>
    /// Note cliente (1.0 à 5.0).
    /// </summary>
    [Range(1.0, 5.0)]
    public decimal? CustomerRating { get; set; }

    public bool CustomerIsBanned { get; set; }

    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    public ICollection<OrderTransaction> OrderTransactions { get; set; }
}
