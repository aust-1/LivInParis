namespace LivInParisRoussilleTeynier.Services;

#region Authentication DTOs

/// <summary>
/// Data required for user login.
/// </summary>
public class LoginDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

/// <summary>
/// Data required for user registration.
/// </summary>
public class RegisterDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

/// <summary>
/// Result of an authentication operation.
/// </summary>
public class AuthResultDto
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

#endregion Authentication DTOs

#region Account DTOs

/// <summary>
/// Data transfer object for an account.
/// </summary>
public class AccountDto
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
}

/// <summary>
/// Data required to update account details.
/// </summary>
public class UpdateAccountDto
{
    public string? UserName { get; set; }
    public string? NewPassword { get; set; }
    public string? CurrentPassword { get; set; }
}

/// <summary>
/// Result of an account update operation.
/// </summary>
public class UpdateAccountResultDto
{
    public bool Success { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}

#endregion Account DTOs

#region Profile DTOs

public class CustomerProfileDto
{
    public int CustomerId { get; set; }
    public string? Username { get; set; }
    public decimal? Rating { get; set; }
    public bool IsBanned { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public AddressDto? Address { get; set; }
    public bool IsCompany { get; set; }
    public string? CompanyName { get; set; }
    public string? ContactFirstName { get; set; }
    public string? ContactLastName { get; set; }
}

public class UpdateCustomerProfileDto
{
    public bool IsBanned { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public UpdateAddressDto? Address { get; set; }
    public bool IsCompany { get; set; }
    public string? CompanyName { get; set; }
    public string? ContactFirstName { get; set; }
    public string? ContactLastName { get; set; }
}

public class ChefProfileDto
{
    public int ChefId { get; set; }
    public string? Username { get; set; }
    public bool IsBanned { get; set; }
    public decimal? Rating { get; set; }
    public AddressDto? Address { get; set; }
}

public class UpdateChefProfileDto
{
    public bool IsBanned { get; set; }
    public UpdateAddressDto? Address { get; set; }
}

#endregion Profile DTOs

#region Address DTOs

public class AddressDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string? Street { get; set; }
}

public class UpdateAddressDto
{
    public int Number { get; set; }
    public string? Street { get; set; }
}

#endregion Address DTOs

#region Dish and Cart DTOs

public class DishDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Cuisine { get; set; }
    public string? Type { get; set; }
    public decimal Price { get; set; }
    public string? PhotoUrl { get; set; }
}

public class DishSearchCriteriaDto
{
    public string? Cuisine { get; set; }
    public string? Type { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

public class CartDto
{
    public int CustomerId { get; set; }
    public IList<CartItemDto>? Items { get; set; }
}

public class CartItemDto
{
    public int DishId { get; set; }
    public string? DishName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

#endregion Dish and Cart DTOs

#region Checkout and Order DTOs

public class CheckoutDto
{
    public int AddressId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Notes { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<OrderLineDto>? Lines { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Status { get; set; }
}

public class OrderLineDto
{
    public int DishId { get; set; }
    public string? DishName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

#endregion Checkout and Order DTOs

#region Menu Proposal DTOs

public class MenuProposalDto
{
    public int ChefId { get; set; }
    public DateTime ProposalDate { get; set; }
    public IList<DishDto>? Dishes { get; set; }
}

public class CreateMenuProposalDto
{
    public DateTime ProposalDate { get; set; }
    public IList<int>? DishIds { get; set; }
}

public class UpdateMenuProposalDto
{
    public IList<int>? DishIds { get; set; }
}

#endregion Menu Proposal DTOs

#region Chef Orders and Delivery DTOs

public class OrderStatusDto
{
    public string? Status { get; set; }
}

public class DeliveryDto
{
    public int DeliveryId { get; set; }
    public int OrderLineId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string? Status { get; set; }
}

#endregion Chef Orders and Delivery DTOs

#region Statistics DTOs

public class ChefDeliveryStatsDto
{
    public int ChefId { get; set; }
    public string? ChefName { get; set; }
    public int DeliveryCount { get; set; }
}

public class RevenueByStreetDto
{
    public string? Street { get; set; }
    public decimal Revenue { get; set; }
}

public class CuisinePreferenceDto
{
    public string? Cuisine { get; set; }
    public int OrderCount { get; set; }
}

#endregion Statistics DTOs

#region Routing DTOs

public class RouteDto
{
    public int OrderId { get; set; }
    public IList<RouteSegmentDto>? Segments { get; set; }
    public decimal TotalDistance { get; set; }
    public TimeSpan EstimatedTime { get; set; }
}

public class RouteSegmentDto
{
    public string? From { get; set; }
    public string? To { get; set; }
    public decimal Distance { get; set; }
    public TimeSpan Duration { get; set; }
}

#endregion Routing DTOs
