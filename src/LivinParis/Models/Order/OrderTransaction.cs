using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("OrderTransaction")]
public class OrderTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    [Required]
    public required DateTime TransactionDatetime { get; set; }

    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public required Customer Customer { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}

//TODO: add doc
