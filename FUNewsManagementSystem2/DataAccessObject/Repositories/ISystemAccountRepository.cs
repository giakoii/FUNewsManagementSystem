using DataAccessObject.Models;

namespace DataAccessObject.Repositories;

public interface ISystemAccountRepository : IRepository<SystemAccount, short>
{
    public SystemAccount GetAccountByEmail(string email);
    
    List<ViewUserNewsHistory> GetNewsHistoriesByAccountId(short id);

    public bool DeleteAccount(short id);
}