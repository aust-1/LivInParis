using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IOrderLine
{
    void CreateOrderLine(
        int orderLineId,
        DateTime orderLineDate,
        int duration,
        Status status,
        bool isEatIn,
        int adressId,
        int transactionId,
        int chefAccountId,
        MySqlCommand? command = null
    );

    List<List<string>> GetOrderLines(
        int limit,
        DateTime? orderLineDate = null,
        int? duration = null,
        Status? status = null,
        bool? isEatIn = null,
        int? adressId = null,
        int? transactionId = null,
        int? chefAccountId = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void UpdateStatusOrderLine(int orderLineId, Status status, MySqlCommand? command = null);

    void DeleteOrderLine(int orderLineId, MySqlCommand? command = null);
}
