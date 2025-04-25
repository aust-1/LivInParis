using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LivInParisRoussilleTeynier.Models.Order.Enums;

namespace LivInParisRoussilleTeynier.Models.Order;

[Table("Review")]
public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewId { get; set; }

    [Required]
    public ReviewerType ReviewType { get; set; }

    [Range(1.0, 5.0)]
    public decimal? ReviewRating { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }

    public required DateTime? ReviewDate { get; set; }

    public int OrderLineId { get; set; }

    [ForeignKey("OrderLineId")]
    public required OrderLine OrderLine { get; set; }
}

//TODO: add doc
