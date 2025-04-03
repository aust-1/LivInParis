using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IOrderLine
{
    void CreateOrderLine(
        int orderLineId,
        DateTime orderLineDate,
        int duration,
        string status,
        bool itsEatIn,
        int adressId,
        int transactionId,
        int accountId,
        MySqlCommand? command = null
    );

    List<List<string>> GetOrderLines(int limit, MySqlCommand? command = null);

    void UpdateStatusOrderLine(int orderLineId, string status, MySqlCommand? command = null);

    void DeleteOrderLine(int orderLineId, MySqlCommand? command = null);
}
