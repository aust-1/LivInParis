using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Customer")]
public class Customer : Account
{
    [Range(1.0, 5.0)]
    public decimal? CustomerRating { get; set; }

    [Required]
    public bool CustomerIsBanned { get; set; }

    public ICollection<OrderTransaction> OrderTransactions { get; set; } =
        new List<OrderTransaction>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

//TODO: add docs

//QUESTION: pertinence reviews ?

//TODO: dans create csharp checker qu'il n'existe pas déjà un company avant de créer un individual
