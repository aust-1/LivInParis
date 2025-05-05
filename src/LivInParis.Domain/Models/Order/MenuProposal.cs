using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

[Table("MenuProposal")]
[Index(
    nameof(AccountId),
    nameof(ProposalDate),
    IsUnique = true,
    Name = "IX_MenuProposal_AccountId_ProposalDate"
)]
public class MenuProposal
{
    [Required]
    public required int AccountId { get; set; }

    [Required]
    public required DateOnly ProposalDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required]
    public required int DishId { get; set; }

    [ForeignKey("AccountId")]
    public Chef? Chef { get; set; }

    [ForeignKey("DishId")]
    public Dish? Dish { get; set; }
}

//TODO: add doc
