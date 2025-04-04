using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IMenuProposalService
{
    void Create(int chefId, DateOnly proposalDate, int dishId, MySqlCommand? command = null);

    List<List<string>> Read(
        int limit,
        int? chefId = null,
        DateOnly? proposalDate = null,
        int? dishId = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Update the menu proposal for a specific chef and date.
    /// </summary>
    /// <param name="chefId"></param>
    /// <param name="proposalDate"></param>
    /// <param name="dishId"></param>
    /// <param name="command"></param>
    void Update(int chefId, DateOnly proposalDate, int dishId, MySqlCommand? command = null);
    void Delete(int chefId, DateOnly proposalDate, MySqlCommand? command = null);
}
