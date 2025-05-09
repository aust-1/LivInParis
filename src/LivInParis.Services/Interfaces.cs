using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Services;

#region Authentication

/// <summary>
/// Defines operations for authentication and session management.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user and returns an authentication token.
    /// </summary>
    Task<AuthResultDto> LoginAsync(LoginDto loginDto);

    /// <summary>
    /// Registers a new user and returns an authentication token.
    /// </summary>
    Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);

    /// <summary>
    /// Logs out the specified user session.
    /// </summary>
    Task LogoutAsync(string token);
}

/// <summary>
/// Defines operations for generating, refreshing and revoking tokens, as well as hashing passwords.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT access token for the specified user.
    /// </summary>
    string GenerateToken(Account user);

    /// <summary>
    /// Generates a refresh token for the specified user.
    /// </summary>
    string GenerateRefreshToken(Account user);

    /// <summary>
    /// Hashes a plain-text password.
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Revokes the specified refresh token, preventing its future use.
    /// </summary>
    Task RevokeTokenAsync(string refreshToken);
}

#endregion

#region Account Management

/// <summary>
/// Defines operations for managing user accounts.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Retrieves account information by identifier.
    /// </summary>
    Task<Account?> GetAccountByIdAsync(int accountId);

    /// <summary>
    /// Updates account details.
    /// </summary>
    /// <param name="updateDto">The data transfer object containing updated account information.</param>
    /// <returns>A task representing the asynchronous operation, containing the updated account information.</returns>
    Task<UpdateAccountResultDto> UpdateAccountAsync(UpdateAccountDto updateDto);

    /// <summary>
    /// Deletes an account.
    /// </summary>
    Task DeleteAccountAsync(int accountId);
}

#endregion

#region Profile Management

/// <summary>
/// Defines operations for managing a customer's profile.
/// </summary>
public interface ICustomerProfileService
{
    /// <summary>
    /// Retrieves the profile for a given customer.
    /// </summary>
    Task<CustomerProfileDto> GetProfileAsync(int customerId);

    /// <summary>
    /// Updates a customer's profile.
    /// </summary>
    /// <param name="customerId">The identifier of the customer.</param>
    /// <param name="updateDto">The data transfer object containing updated profile information.</param>
    /// <returns>A task representing the asynchronous operation, containing the updated profile information.</returns>
    Task UpdateProfileAsync(int customerId, UpdateCustomerProfileDto updateDto);
}

/// <summary>
/// Defines operations for managing a chef's profile.
/// </summary>
public interface IChefProfileService
{
    /// <summary>
    /// Retrieves the profile for a given chef.
    /// </summary>
    Task<ChefProfileDto> GetProfileAsync(int chefId);

    /// <summary>
    /// Updates a chef's profile.
    /// </summary>
    Task UpdateProfileAsync(int chefId, UpdateChefProfileDto updateDto);
}

#endregion

#region Dishes and Cart

/// <summary>
/// Defines operations for browsing and searching dishes.
/// </summary>
public interface IDishService
{
    /// <summary>
    /// Retrieves all available dishes.
    /// </summary>
    Task<IEnumerable<DishDto>> GetAllDishesAsync();

    /// <summary>
    /// Retrieves a dish by its identifier.
    /// </summary>
    Task<DishDto?> GetDishByIdAsync(int dishId);

    /// <summary>
    /// Searches dishes based on criteria such as cuisine or type.
    /// </summary>
    Task<IEnumerable<DishDto>> SearchDishesAsync(DishSearchCriteriaDto criteria);
}

/// <summary>
/// Defines operations for managing a customer's shopping cart.
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Retrieves the current shopping cart for a customer.
    /// </summary>
    Task<CartDto> GetCartAsync(int customerId);

    /// <summary>
    /// Adds an item to the shopping cart.
    /// </summary>
    Task AddItemAsync(int customerId, int chefId);

    /// <summary>
    /// Removes an item from the shopping cart.
    /// </summary>
    Task RemoveItemAsync(int customerId, int chefId);

    /// <summary>
    /// Clears all items from the shopping cart.
    /// </summary>
    Task ClearCartAsync(int customerId);
}

#endregion

#region Orders and Checkout

/// <summary>
/// Defines operations for placing orders and checkout.
/// </summary>
public interface ICheckoutService
{
    /// <summary>
    /// Places an order based on the customer's cart and checkout details.
    /// </summary>
    Task<TransactionDto> PlaceOrderAsync(int customerId, CheckoutDto checkoutDto);
}

/// <summary>
/// Defines operations for retrieving and managing customer orders.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Retrieves all orders for a specific customer.
    /// </summary>
    Task<IEnumerable<TransactionDto>> GetTransactionsByCustomerAsync(int customerId);

    /// <summary>
    /// Retrieves order details by order identifier.
    /// </summary>
    Task<TransactionDto> GetTransactionByIdAsync(int transactionId);
}

/// <summary>
/// Defines operations for retrieving and managing customer orders.
/// </summary>
public interface IOrderlineService
{
    /// <summary>
    /// Retrieves all order lines for a specific transaction.
    /// </summary>
    /// <param name="transactionId">The identifier of the transaction.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of order lines.</returns>
    Task<IEnumerable<TransactionDto>> GetOrderLinesByTransactionAsync(int transactionId);

    /// <summary>
    /// Retrieves order line details by orderline identifier.
    /// </summary>
    /// <param name="orderLineId">The identifier of the order line.</param>
    /// <returns>A task representing the asynchronous operation, containing the order line details.</returns>
    Task<OrderLineDto> GetOrderLineByIdAsync(int orderLineId);

    /// <summary>
    /// Cancels an existing order line.
    /// </summary>
    /// <param name="orderLineId">The identifier of the order line to cancel.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CancelOrderLineAsync(int orderLineId);
}

#endregion

#region Chef Operations

/// <summary>
/// Defines operations for managing menu proposals by chefs.
/// </summary>
public interface IMenuProposalService
{
    /// <summary>
    /// Retrieves all menu proposals for a chef.
    /// </summary>
    Task<IEnumerable<MenuProposalDto>> GetProposalsByChefAsync(int chefId);

    /// <summary>
    /// Creates a new menu proposal.
    /// </summary>
    Task CreateProposalAsync(CreateMenuProposalDto createDto);

    /// <summary>
    /// Deletes a menu proposal.
    /// </summary>
    Task DeleteProposalAsync(int chefId, DateTime proposalDate);
}

/// <summary>
/// Defines operations for handling incoming orders assigned to chefs.
/// </summary>
public interface IIncomingOrderService
{
    /// <summary>
    /// Retrieves pending orders for a chef.
    /// </summary>
    Task<IEnumerable<OrderLineDto>> GetIncomingOrdersAsync(int chefId);

    /// <summary>
    /// Accepts an incoming order.
    /// </summary>
    Task AcceptOrderAsync(int orderId);

    /// <summary>
    /// Rejects an incoming order.
    /// </summary>
    Task RejectOrderAsync(int orderId);

    /// <summary>
    /// Updates the status of an order (e.g., prepared, delivering).
    /// </summary>
    Task UpdateOrderStatusAsync(int orderId, OrderStatusDto statusDto);
}

/// <summary>
/// Defines operations for managing delivery tracking for chefs.
/// </summary>
public interface IDeliveryService
{
    /// <summary>
    /// Retrieves deliveries assigned to a chef.
    /// </summary>
    Task<IEnumerable<DeliveryDto>> GetDeliveriesByChefAsync(int chefId);

    /// <summary>
    /// Retrieves details of a specific delivery.
    /// </summary>
    Task<DeliveryDto> GetDeliveryDetailAsync(int deliveryId);
}

#endregion

#region Statistics

/// <summary>
/// Defines operations for retrieving statistical insights.
/// </summary>
public interface IStatisticsService
{
    /// <summary>
    /// Retrieves count of deliveries per chef.
    /// </summary>
    Task<IEnumerable<ChefDeliveryStatsDto>> GetChefDeliveryStatsAsync();

    /// <summary>
    /// Retrieves revenue grouped by street.
    /// </summary>
    Task<IEnumerable<RevenueByStreetDto>> GetRevenueByStreetAsync();

    /// <summary>
    /// Retrieves the average order price across all orders.
    /// </summary>
    Task<decimal> GetAverageOrderPriceAsync();

    /// <summary>
    /// Retrieves top cuisines by number of orders in the given period.
    /// </summary>
    Task<IEnumerable<CuisinePreferenceDto>> GetTopCuisinesAsync(
        DateTime? from = null,
        DateTime? to = null
    );
}

#endregion

#region Routing

/// <summary>
/// Defines operations for calculating delivery routes.
/// </summary>
public interface IRouteCalculator
{
    /// <summary>
    /// Calculates the optimal route for a given order.
    /// </summary>
    Task<RouteDto> CalculateRouteAsync(int orderId);
}

#endregion
