using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Customer")]
public class Customer
{
    [Key]
    public int AccountId { get; set; }

    [Range(1.0, 5.0)]
    public decimal? CustomerRating { get; set; }

    [Required]
    public bool CustomerIsBanned { get; set; }

    [ForeignKey("AccountId")]
    public required Account Account { get; set; }

    public ICollection<OrderTransaction> OrderTransactions { get; set; } =
        new List<OrderTransaction>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

//TODO: add docs

//HACK: refactor have company and individual in customer and not in account

//QUESTION: pertinence reviews ?
