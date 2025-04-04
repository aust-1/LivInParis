using LivinParisRoussilleTeynier.Data.Services;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Tests;

[TestClass]
public class AccountServiceTests
{
    private const string ConnectionString =
        "server=localhost;user=root;password=root;database=livinparis_test;";
    private MySqlConnection _connection = null!;
    private MySqlTransaction _transaction = null!;
    private AccountService _accountService = null!;

    [TestInitialize]
    public void Setup()
    {
        _connection = new MySqlConnection(ConnectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();
        _accountService = new AccountService();
    }

    [TestCleanup]
    public void Teardown()
    {
        _transaction.Rollback(); // rollback des changements
        _connection.Close();
    }

    [TestMethod]
    public void TestCreateAndRead()
    {
        var cmd = _connection.CreateCommand();
        cmd.Transaction = _transaction;

        _accountService.Create(1, "test@example.com", "password123", cmd);

        var result = _accountService.Read(10, cmd);
        bool found = result.Exists(row => row.Contains("test@example.com"));

        Assert.IsTrue(found, "Le compte n'a pas été trouvé dans la lecture.");
    }

    [TestMethod]
    public void TestUpdateEmail()
    {
        var cmd = _connection.CreateCommand();
        cmd.Transaction = _transaction;

        _accountService.Create(2, "old@example.com", "pwd", cmd);
        _accountService.UpdateEmail(2, "new@example.com", cmd);

        var result = _accountService.Read(10, cmd);
        bool found = result.Exists(row => row.Contains("new@example.com"));

        Assert.IsTrue(found, "L'adresse e-mail n'a pas été mise à jour.");
    }

    [TestMethod]
    public void TestUpdatePassword()
    {
        var cmd = _connection.CreateCommand();
        cmd.Transaction = _transaction;

        _accountService.Create(3, "pwd@example.com", "oldpwd", cmd);
        _accountService.UpdatePassword(3, "newpwd", cmd);

        var updatedRows = _accountService.Read(10, cmd);
        var updated = updatedRows.Find(row => row.Contains("pwd@example.com"));

        Assert.IsNotNull(updated);
        Assert.AreEqual("newpwd", updated?[2], "Le mot de passe n'a pas été mis à jour.");
    }

    [TestMethod]
    public void TestDelete()
    {
        var cmd = _connection.CreateCommand();
        cmd.Transaction = _transaction;

        _accountService.Create(4, "delete@example.com", "toDelete", cmd);
        _accountService.Delete("delete@example.com", cmd);

        var result = _accountService.Read(10, cmd);
        bool found = result.Exists(row => row.Contains("delete@example.com"));

        Assert.IsFalse(found, "Le compte n'a pas été supprimé.");
    }
}
