using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IContainsService
{
    void Create(int ingredientId, int dishId, MySqlCommand? command = null);

    List<int> GetIngredientsByDishId(int limit, int dishId, MySqlCommand? command = null);

    List<int> GetDishesByIngredientId(int limit, int ingredientId, MySqlCommand? command = null);

    void Delete(int ingredientId, int dishId, MySqlCommand? command = null);
}
