using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivinParisRoussilleTeynier.Models.Order;

[Table("Dish")]
public class Dish
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DishId { get; set; }

    [Required]
    [MaxLength(50)]
    public string DishName { get; set; }

    [Required]
    public DishType DishType { get; set; }

    [Required]
    public int ExpiryTime { get; set; }

    [Required]
    [MaxLength(50)]
    public string CuisineNationality { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    public ProductOrigin ProductOrigin { get; set; }

    [MaxLength(255)]
    public string PhotoPath { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
    public ICollection<MenuProposal> MenuProposals { get; set; }
    public ICollection<Contains> Contains { get; set; }
}

[Table("Customer")]
public class Customer
{
    [Key]
    public int AccountId { get; set; }

    /// <summary>
    /// Note cliente (1.0 à 5.0).
    /// </summary>
    [Range(1.0, 5.0)]
    public decimal? CustomerRating { get; set; }

    public bool CustomerIsBanned { get; set; }

    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    public ICollection<OrderTransaction> OrderTransactions { get; set; }
}

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

[Table("OrderTransaction")]
public class OrderTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    [Required]
    public DateTime TransactionDatetime { get; set; }

    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public Customer Customer { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
}

[Table("Company")]
public class Company
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CompanyName { get; set; }

    [MaxLength(50)]
    public string ContactFirstName { get; set; }

    [MaxLength(50)]
    public string ContactLastName { get; set; }

    [ForeignKey("AccountId")]
    public Customer Customer { get; set; }
}

[Table("Individual")]
public class Individual
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string PersonalEmail { get; set; }

    [Required]
    [MaxLength(50)]
    public string PhoneNumber { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public Address Address { get; set; }

    [ForeignKey("AccountId")]
    public Customer Customer { get; set; }
}

[Table("OrderLine")]
public class OrderLine
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderLineId { get; set; }

    [Required]
    public DateTime OrderLineDatetime { get; set; }

    [Required]
    public OrderLineStatus OrderLineStatus { get; set; }

    public bool IsEatIn { get; set; }

    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    public Address Address { get; set; }

    public int TransactionId { get; set; }

    [ForeignKey("TransactionId")]
    public OrderTransaction OrderTransaction { get; set; }

    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public Chef Chef { get; set; }
}

[Table("Review")]
public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewId { get; set; }

    [Required]
    public ReviewType ReviewType { get; set; }

    [Range(1.0, 5.0)]
    public decimal? ReviewRating { get; set; }

    [MaxLength(500)]
    public string Comment { get; set; }

    public DateTime? ReviewDate { get; set; }

    public int OrderLineId { get; set; }

    [ForeignKey("OrderLineId")]
    public OrderLine OrderLine { get; set; }
}

[Table("MenuProposal")]
public class MenuProposal
{
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    public Chef Chef { get; set; }

    public DateTime ProposalDate { get; set; }

    public int DishId { get; set; }

    [ForeignKey("DishId")]
    public Dish Dish { get; set; }
}

[Table("Contains")]
public class Contains
{
    public int IngredientId { get; set; }

    [ForeignKey("IngredientId")]
    public Ingredient Ingredient { get; set; }

    public int DishId { get; set; }

    [ForeignKey("DishId")]
    public Dish Dish { get; set; }
}
