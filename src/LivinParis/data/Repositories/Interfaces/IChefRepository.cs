using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IChef
{
    void CreateChef(
        int accountId,
        decimal chefRating,
        bool eatsOnSite,
        bool chefIsBanned,
        int adressId,
        MySqlCommand? command = null
    );

    Dictionary<int, List<string>> GetChefs(
        int limit,
        decimal? ratingHigherThan = null,
        decimal? ratingBelow = null,
        bool? eatsOnSite = null,
        bool? chefIsBanned = null,
        string? orderBy = null,
        MySqlCommand? command = null
    );

    void UpdateChefRating(int accountId, decimal chefRating, MySqlCommand? command = null);

    void UpdateEatsOnSite(int accountId, bool eatsOnSite, MySqlCommand? command = null);

    void UpdateChefIsBanned(int accountId, bool chefIsBanned, MySqlCommand? command = null);

    void DeleteChef(int accountId, MySqlCommand? command = null);
}
