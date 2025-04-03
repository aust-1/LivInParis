// using MySql.Data.MySqlClient;

// namespace LivinParis.Data;

// public static class TransactionRepository : ITransaction
// {
//     private static MySqlConnection? s_connection;

//     public static bool InitConnection()
//     {
//         try
//         {
//             string connection =
//                 "SERVER=localhost;PORT=3306;DATABASE=PSI;UID=eliottfrancois;PASSWORD=PSI";
//             s_connection = new MySqlConnection(connection);
//             return true;
//         }
//         catch (Exception)
//         {
//             return false;
//         }
//     }

//     public static void OpenConnection()
//     {
//         s_connection!.Open();
//     }

//     public static void CloseConnection()
//     {
//         s_connection!.Close();
//     }

//     ///CRUD

//     public static void CreateTransaction(int transactionId, DateTime transactionDate, int accountId)
//     {
//         OpenConnection();
//         using var command = new MySqlCommand();
//         command.CommandText = "INSERT INTO account VALUES (@t, @d, @i)";
//         command.Parameters.AddWithValue("@t", transactionId);
//         command.Parameters.AddWithValue("@d", transactionDate);
//         command.Parameters.AddWithValue("@i", accountId);
//         command.ExecuteNonQuery();
//         CloseConnection();
//     }

//     public static List<List<string>> GetTransaction(int limit)
//     {
//         OpenConnection();
//         using var command = new MySqlCommand();
//         command.CommandText = "SELECT * FROM Transaction LIMIT @l";
//         command.Parameters.AddWithValue("@l", limit);

//         using var reader = command.ExecuteReader();
//         List<List<string>> transactions = [];
//         while (reader.Read())
//         {
//             List<string> transaction = [];
//             for (int i = 0; i < reader.FieldCount; i++)
//             {
//                 string value = reader[i]?.ToString() ?? string.Empty;
//                 transaction.Add(value);
//             }
//             transactions.Add(transaction);
//         }
//         return transactions;
//     }

//     public static void UpdateTransactionDate(int transactionId, DateTime date)
//     {
//         OpenConnection();
//         using var command = new MySqlCommand();
//         command.CommandText =
//             "UPDATE account SET transaction_datetime = @d WHERE transaction_id = @t";
//         command.Parameters.AddWithValue("@d", date);
//         command.Parameters.AddWithValue("@t", transactionId);
//         command.ExecuteNonQuery();
//         CloseConnection();
//     }

//     public static void DeleteTransaction(int transactionId)
//     {
//         OpenConnection();
//         using var command = new MySqlCommand();
//         command.CommandText = "DELETE FROM account WHERE transaction_id = @t";
//         command.Parameters.AddWithValue("@t", transactionId);
//         command.ExecuteNonQuery();
//         CloseConnection();
//     }
// }
