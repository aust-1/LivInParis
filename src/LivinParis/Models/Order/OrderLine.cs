using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [Required]
    public required int AddressId { get; set; }

    [Required]
    public required int TransactionId { get; set; }

    /// <summary>
    /// The ID of the chef account that prepared the order.
    /// </summary>
    [Required]
    public required int AccountId { get; set; }

    [ForeignKey("AddressId")]
    public Address? Address { get; set; }

    [ForeignKey("TransactionId")]
    public OrderTransaction? OrderTransaction { get; set; }

    [ForeignKey("AccountId")]
    public Chef? Chef { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

//TODO: add doc
