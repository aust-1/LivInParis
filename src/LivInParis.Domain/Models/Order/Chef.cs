using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

[Table("Chef")]
public class Chef
{
    [Key]
    public int AccountId { get; set; }

    [Range(1.0, 5.0)]
    public decimal? ChefRating { get; set; }

    [Required]
    public required bool ChefIsBanned { get; set; }

    [Required]
    public required int AddressId { get; set; }

    [ForeignKey("AccountId")]
    public Account? Account { get; set; }

    [ForeignKey("AddressId")]
    public Address? Address { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    public ICollection<MenuProposal> MenuProposals { get; set; } = new List<MenuProposal>();
}


//QUESTION: pertinence reviews ?

//TODO: add doc
