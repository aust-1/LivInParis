using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

/// <summary>
/// Represents a chef in the system.
/// </summary>
[Table("Chef")]
public class Chef
{
    /// <summary>
    /// The primary key for the chef account.
    /// </summary>
    [Key]
    [Column("account_id")]
    public int ChefAccountId { get; set; }

    /// <summary>
    /// Indicates whether the chef is banned from the platform.
    /// </summary>
    [Required]
    [Column("chef_is_banned")]
    public bool ChefIsBanned { get; set; }

    /// <summary>
    /// The ID of the address associated with the chef.
    /// </summary>
    [Required]
    [Column("address_id")]
    public required int AddressId { get; set; }

    /// <summary>
    /// The Account associated with the chef.
    /// </summary>
    [ForeignKey("ChefAccountId")]
    public Account? Account { get; set; }

    /// <summary>
    /// The address associated with the chef.
    /// </summary>
    [ForeignKey("AddressId")]
    public Address? Address { get; set; }

    /// <summary>
    /// The orderlines associated with the chef.
    /// </summary>
    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    /// <summary>
    /// The menu proposals associated with the chef.
    /// </summary>
    public ICollection<MenuProposal> MenuProposals { get; set; } = new List<MenuProposal>();
}
