using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

[Table("OrderTransaction")]
public class OrderTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    [Required]
    public required DateTime TransactionDatetime { get; set; }

    [Required]
    public required int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public Customer? Customer { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}

//TODO: add doc
