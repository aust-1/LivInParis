using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("MenuProposal")]
[Index(
    nameof(AccountId),
    nameof(ProposalDate),
    IsUnique = true,
    Name = "IX_MenuProposal_AccountId_ProposalDate"
)]
public class MenuProposal
{
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public required Chef Chef { get; set; }

    public required DateTime ProposalDate { get; set; }

    public int DishId { get; set; }

    [ForeignKey("DishId")]
    public required Dish Dish { get; set; }
}

//TODO: add doc
