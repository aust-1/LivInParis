using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IChef
{
    void CreateChef(
        int chefAccountId,
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
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void UpdateChefRating(int chefAccountId, decimal chefRating, MySqlCommand? command = null);

    void UpdateEatsOnSite(int chefAccountId, bool eatsOnSite, MySqlCommand? command = null);

    void UpdateChefIsBanned(int chefAccountId, bool chefIsBanned, MySqlCommand? command = null);

    void DeleteChef(int chefAccountId, MySqlCommand? command = null);
}
