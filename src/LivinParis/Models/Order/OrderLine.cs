using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("OrderLine")]
public class OrderLine
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderLineId { get; set; }

    [Required]
    public DateTime OrderLineDatetime { get; set; }

    [Required]
    public OrderLineStatus OrderLineStatus { get; set; }

    public bool IsEatIn { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public Address Address { get; set; }

    public int TransactionId { get; set; }

    [ForeignKey("TransactionId")]
    public OrderTransaction OrderTransaction { get; set; }

    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public Chef Chef { get; set; }
}
