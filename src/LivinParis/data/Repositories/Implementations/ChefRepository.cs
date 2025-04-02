namespace LivinParis.Data;
using MySql.Data.MySqlClient;

public class ChefRepository : IChef
{
    private static MySqlConnection? conn;

    public static bool InitConnection()
    {
        try
        {
            string connection = "SERVER=localhost;PORT=3306;DATABASE=nom_database;UID=eliottfrancois;PASSWORD=PSI";
            MySqlConnection conn = new MySqlConnection(connection);
            conn.Open();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        
    }

    public static void CloseConnection()
    {
        conn?.Close();
    }
    
}