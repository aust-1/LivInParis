using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Chef")]
public class Chef
{
    [Key]
    public int AccountId { get; set; }

    [Range(1.0, 5.0)]
    public decimal? ChefRating { get; set; }

    public bool ChefIsBanned { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public required Address Address { get; set; }

    [ForeignKey("AccountId")]
    public required Account Account { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    public ICollection<MenuProposal> MenuProposals { get; set; } = new List<MenuProposal>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}


//QUESTION: pertinence reviews ?

//TODO: add doc
