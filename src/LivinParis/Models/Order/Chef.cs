using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Chef")]
public class Chef
{
    [Key]
    public int AccountId { get; set; }

    [Range(1.0, 5.0)]
    public decimal? ChefRating { get; set; }

    public bool EatsOnSite { get; set; }
    public bool ChefIsBanned { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public Address Address { get; set; }

    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
    public ICollection<MenuProposal> MenuProposals { get; set; }
}
