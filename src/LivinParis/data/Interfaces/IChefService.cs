using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IChefService
{
    void Create(
        int chefAccountId,
        decimal chefRating,
        bool eatsOnSite,
        bool chefIsBanned,
        int addressId,
        MySqlCommand? command = null
    );

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

    void UpdateRating(int chefAccountId, decimal chefRating, MySqlCommand? command = null);

    void UpdateEatsOnSite(int chefAccountId, bool eatsOnSite, MySqlCommand? command = null);

    void UpdateIsBanned(int chefAccountId, bool chefIsBanned, MySqlCommand? command = null);

    void Delete(int chefAccountId, MySqlCommand? command = null);
}
