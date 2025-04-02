namespace LivinParis.Data;

interface IAccount
{
    [ConnectionControl]
    public virtual void CreateAccount(int accountId, string email, string password)
    {
        throw new NotImplementedException();
    }

    [ConnectionControl]
    public virtual List<List<string>> GetAccounts(int limit)
    {
        throw new NotImplementedException();
    }

    [ConnectionControl]
    public virtual void UpdateEmail(int accountId, string email)
    {
        throw new NotImplementedException();
    }

    [ConnectionControl]
    public virtual void UpdatePassword(int accountId, string password)
    {
        throw new NotImplementedException();
    }

    [ConnectionControl]
    public virtual void DeleteAccount(int accountId)
    {
        throw new NotImplementedException();
    }
}
