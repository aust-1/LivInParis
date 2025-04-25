using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LivInParisRoussilleTeynier.Models.Order.Enums;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("OrderLine")]
public class OrderLine
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderLineId { get; set; }

    [Required]
    public required DateTime OrderLineDatetime { get; set; }

    [Required]
    public required OrderLineStatus OrderLineStatus { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public required Address Address { get; set; }

    public int TransactionId { get; set; }

    [ForeignKey("TransactionId")]
    public required OrderTransaction OrderTransaction { get; set; }

    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public required Chef Chef { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

//TODO: add doc
