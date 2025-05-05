using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;

namespace LivInParisRoussilleTeynier.Domain.Models.Order;

[Table("Review")]
public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewId { get; set; }

    [Required]
    public ReviewerType ReviewerType { get; set; }

    [Range(1.0, 5.0)]
    public decimal? ReviewRating { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }

    [Required]
    public required DateOnly? ReviewDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required]
    public required int OrderLineId { get; set; }

    [ForeignKey("OrderLineId")]
    public OrderLine? OrderLine { get; set; }
}

//TODO: add doc
