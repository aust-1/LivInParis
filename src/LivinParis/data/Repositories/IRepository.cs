using LivinParis.Models.Order;

namespace LivinParis.Repositories;

//C'est le copié collé de morgan, tu dois 100% refaire

interface IRepository
{
    public static void CreateCustomer()
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetCustomers(int limit)
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetChefs(int limit)
    {
        throw new NotImplementedException();
    }

    public static void AddChef()
    {
        throw new NotImplementedException();
    }

    public static List<string> GetCustomerInfo(string email)
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetCustomerOrders(string email, int limit)
    {
        throw new NotImplementedException();
    }

    public static int GetCustomerOrdersCount(string email)
    {
        throw new NotImplementedException();
    }

    public static void UpdateCustomer(string email, string field, string value)
    {
        throw new NotImplementedException();
    }

    public static bool CheckUser(string email, string table)
    {
        throw new NotImplementedException();
    }

    public static bool CheckPassword(string email, string password, string table)
    {
        throw new NotImplementedException();
    }

    public static (List<string>, List<int>) GetDishesNamesAndPrices()
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, List<Ingredient>> GetDishesIngredients()
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, List<string>> GetDishesAllergens()
    {
        throw new NotImplementedException();
    }

    public static bool ConfirmOrder(Dictionary<string, int> order, float price, string email)
    {
        throw new NotImplementedException();
    }

    public static string GetRandomChefEmail()
    {
        throw new NotImplementedException();
    }

    public static bool AddIngredient(Ingredient ingredient, string email)
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetChefOrders(string email, int limit)
    {
        throw new NotImplementedException();
    }

    public static void UpdateOrderStatus(string email, string order_time, string newStatus)
    {
        throw new NotImplementedException();
    }

    public static (Dictionary<string, int>, int) GetRecordsCount()
    {
        throw new NotImplementedException();
    }
}
