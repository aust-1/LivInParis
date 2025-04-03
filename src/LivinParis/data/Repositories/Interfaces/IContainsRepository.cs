using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IContains
{
    void CreateContains(int ingredientId, int dishId, MySqlCommand? command = null);

    List<int> GetIngredientsByDishId(int limit, int dishId, MySqlCommand? command = null);

    List<int> GetDishesByIngredientId(int limit, int ingredientId, MySqlCommand? command = null);

    void DeleteContains(int ingredientId, int dishId, MySqlCommand? command = null);
}
