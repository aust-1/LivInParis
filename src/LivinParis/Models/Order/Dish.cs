namespace LivinParisRoussilleTeynier.Models.Order;

public class Dish(
    string dishName,
    string dishType,
    int preparationTime,
    int expirationTime,
    string cuisineNationality,
    int quantity,
    float price,
    string photoPath,
    List<Ingredient> ingredients
)
{
    public string DishName { get; set; } = dishName;
    public string DishType { get; set; } = dishType;
    public int PreparationTime { get; set; } = preparationTime;
    public int ExpirationTime { get; set; } = expirationTime;
    public string CuisineNationality { get; set; } = cuisineNationality;
    public int Quantity { get; set; } = quantity;
    public float Price { get; set; } = price;
    public string PhotoPath { get; set; } = photoPath;
    public List<Ingredient> Ingredients { get; set; } = ingredients;
}
