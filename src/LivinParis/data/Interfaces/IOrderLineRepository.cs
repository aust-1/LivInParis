using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IOrderLine
{
    void CreateOrderLine(
        int orderLineId,
        DateTime orderLineDate,
        int duration,
        OrderLineStatus orderLineStatus,
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
        OrderLineStatus? orderLineStatus = null,
        bool? isEatIn = null,
        int? adressId = null,
        int? transactionId = null,
        int? chefAccountId = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void UpdateOrderLineStatus(
        int orderLineId,
        OrderLineStatus orderLineStatus,
        MySqlCommand? command = null
    );

    void DeleteOrderLine(int orderLineId, MySqlCommand? command = null);
}
