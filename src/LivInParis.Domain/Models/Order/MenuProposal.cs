using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

/// <summary>
/// Represents a menu proposal in the system.
/// </summary>
[Table("MenuProposal")]
[Index(
    nameof(ChefAccountId),
    nameof(ProposalDate),
    IsUnique = true,
    Name = "IX_MenuProposal_AccountId_ProposalDate"
)]
public class MenuProposal
{
    /// <summary>
    /// One of the primary keys for the MenuProposal table.
    /// The Id of the chef associated with this proposal.
    /// </summary>
    [Required]
    [Column("account_id")]
    public required int ChefAccountId { get; set; }

    /// <summary>
    /// One of the primary keys for the MenuProposal table.
    /// The date of the proposal.
    /// </summary>
    [Required]
    [Column("proposal_date", TypeName = "date")]
    public required DateOnly ProposalDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    /// <summary>
    /// The Id of the dish associated with this proposal.
    /// </summary>
    [Required]
    [Column("dish_id")]
    public required int DishId { get; set; }

    /// <summary>
    /// The Chef associated with this proposal.
    /// </summary>
    [ForeignKey("ChefAccountId")]
    public Chef? Chef { get; set; }

    /// <summary>
    /// The Dish associated with this proposal.
    /// </summary>
    [ForeignKey("DishId")]
    public Dish? Dish { get; set; }
}
