using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Chef")]
public class Chef : Account
{
    [Range(1.0, 5.0)]
    public decimal? ChefRating { get; set; }

    [Required]
    public required bool ChefIsBanned { get; set; }

    [Required]
    public required int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public required Address Address { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    public ICollection<MenuProposal> MenuProposals { get; set; } = new List<MenuProposal>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}


//QUESTION: pertinence reviews ?

//TODO: add doc
