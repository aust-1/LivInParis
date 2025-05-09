namespace LivInParisRoussilleTeynier.Api;

public record LoginRequest(string Name, string Password);

public record RegisterRequest(string Name, string Password);

public record OrderRequest(
    int Id,
    int CustomerId,
    int ChefId,
    DateTime Date,
    List<OrderLineRequest> Items
);

public record OrderLineRequest(int DishId, int Quantity);
