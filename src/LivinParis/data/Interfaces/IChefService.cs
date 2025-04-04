using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing chef entities in the database.
/// </summary>
public interface IChefService
{
    #region CRUD
    //TODO: stat argent gagné

    /// <summary>
    /// Creates a new chef account with the specified information.
    /// </summary>
    /// <param name="chefAccountId">The account ID of the chef (linked to the Account table).</param>
    /// <param name="chefRating">The initial rating of the chef (between 1.0 and 5.0).</param>
    /// <param name="eatsOnSite">Indicates whether the chef allows eating on site.</param>
    /// <param name="chefIsBanned">Indicates whether the chef is currently banned.</param>
    /// <param name="addressId">The address ID where the chef is located.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Create(
        int chefAccountId,
        decimal chefRating,
        bool eatsOnSite,
        bool chefIsBanned,
        int addressId,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Retrieves a list of chefs, optionally filtered by rating, ban status, or eating-on-site option.
    /// </summary>
    /// <param name="limit">Maximum number of rows to return.</param>
    /// <param name="minRating">Optional minimum rating to filter chefs.</param>
    /// <param name="maxRating">Optional maximum rating to filter chefs.</param>
    /// <param name="eatsOnSite">Optional filter to select chefs who allow eating on site.</param>
    /// <param name="chefIsBanned">Optional filter to select chefs who are banned or not.</param>
    /// <param name="orderBy">Optional column to order the result set by.</param>
    /// <param name="orderDirection">True for ascending, false for descending ordering.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing chef rows.</returns>
    List<List<string>> Read(
        int limit,
        decimal? minRating = null,
        decimal? maxRating = null,
        bool? eatsOnSite = null,
        bool? chefIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the rating of a specific chef.
    /// </summary>
    /// <param name="chefAccountId">The ID of the chef account.</param>
    /// <param name="chefRating">The new rating to assign to the chef.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateRating(int chefAccountId, decimal chefRating, MySqlCommand? command = null);

    /// <summary>
    /// Updates whether a chef allows eating on site.
    /// </summary>
    /// <param name="chefAccountId">The ID of the chef account.</param>
    /// <param name="eatsOnSite">The new value indicating if eating on site is allowed.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateEatsOnSite(int chefAccountId, bool eatsOnSite, MySqlCommand? command = null);

    /// <summary>
    /// Updates the ban status of a chef.
    /// </summary>
    /// <param name="chefAccountId">The ID of the chef account.</param>
    /// <param name="chefIsBanned">The new ban status of the chef.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateIsBanned(int chefAccountId, bool chefIsBanned, MySqlCommand? command = null);

    /// <summary>
    /// Deletes a chef from the database.
    /// </summary>
    /// <param name="chefAccountId">The ID of the chef account to delete.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int chefAccountId, MySqlCommand? command = null);

    #endregion CRUD
}
