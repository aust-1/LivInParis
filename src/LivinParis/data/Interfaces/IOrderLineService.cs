using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IOrderLineService
{
    void Create(
        int orderLineId,
        DateTime orderLineDate,
        int duration,
        OrderLineStatus orderLineStatus,
        bool isEatIn,
        int addressId,
        int transactionId,
        int chefAccountId,
        MySqlCommand? command = null
    );

    List<List<string>> Read(
        int limit,
        DateTime? orderLineDate = null,
        int? duration = null,
        OrderLineStatus? orderLineStatus = null,
        bool? isEatIn = null,
        int? addressId = null,
        int? transactionId = null,
        int? chefAccountId = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    List<List<string>> GetCommandCountByStreet(int limit, MySqlCommand? command = null);

    List<List<string>> GetCommandCountByPostalCode(int limit, MySqlCommand? command = null);

    List<List<string>> GetTotalOrderValueByStreet(int limit, MySqlCommand? command = null);

    List<List<string>> GetTotalOrderValueByPostalCode(int limit, MySqlCommand? command = null);

    List<List<string>> GetMostOrderedHours(int limit, MySqlCommand? command = null);

    List<List<string>> GetMostOrderedWeekdays(int limit, MySqlCommand? command = null);

    List<List<string>> GetAverageOrderDuration(int limit, MySqlCommand? command = null);

    void UpdateStatus(
        int orderLineId,
        OrderLineStatus orderLineStatus,
        MySqlCommand? command = null
    );

    void Delete(int orderLineId, MySqlCommand? command = null);
}
