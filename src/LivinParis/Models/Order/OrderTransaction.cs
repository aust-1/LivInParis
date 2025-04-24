using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("OrderTransaction")]
public class OrderTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    [Required]
    public DateTime TransactionDatetime { get; set; }

    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public Customer Customer { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
}
