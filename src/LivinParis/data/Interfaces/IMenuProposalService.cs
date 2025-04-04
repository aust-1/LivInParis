using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage menu proposals for chefs.
/// </summary>
public interface IMenuProposalService
{
    #region CRUD

    /// <summary>
    /// Creates a new menu proposal for a specific chef and date.
    /// </summary>
    /// <param name="chefId">The ID of the chef making the proposal.</param>
    /// <param name="proposalDate">The date of the proposal.</param>
    /// <param name="dishId">The ID of the dish being proposed.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Create(int chefId, DateOnly proposalDate, int dishId, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves a list of menu proposals with optional filters.
    /// </summary>
    /// <param name="limit">The maximum number of rows to return.</param>
    /// <param name="chefId">Filter by chef ID.</param>
    /// <param name="proposalDate">Filter by proposal date.</param>
    /// <param name="dishId">Filter by dish ID.</param>
    /// <param name="orderBy">Column to order by.</param>
    /// <param name="orderDirection">Order direction (true for ascending, false for descending).</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing menu proposal rows.</returns>
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
    /// <param name="chefId">The ID of the chef making the proposal.</param>
    /// <param name="proposalDate">The date of the proposal.</param>
    /// <param name="dishId">The ID of the new dish being proposed.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Update(int chefId, DateOnly proposalDate, int dishId, MySqlCommand? command = null);

    /// <summary>
    /// Deletes a menu proposal for a specific chef and date.
    /// </summary>
    /// <param name="chefId"> The ID of the chef making the proposal.</param>
    /// <param name="proposalDate">The date of the proposal.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int chefId, DateOnly proposalDate, MySqlCommand? command = null);

    #endregion CRUD

    #region Statistics

    /// <summary>
    /// Retrieves the dishes proposed by a specific chef, ordered by frequency of proposal.
    /// </summary>
    /// <param name="limit">The maximum number of dishes to return.</param>
    /// <param name="chefId">The ID of the chef.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>>A list of lists of strings representing the dishes proposed by the chef.</returns>
    List<List<string>> GetDishesByChefFrequency(
        int limit,
        int chefId,
        MySqlCommand? command = null
    );

    #endregion Statistics
}
