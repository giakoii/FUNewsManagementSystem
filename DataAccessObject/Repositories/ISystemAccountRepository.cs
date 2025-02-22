using DataAccessObject.Models;

namespace DataAccessObject.Repositories;

public interface ISystemAccountRepository : IRepository<SystemAccount>
{
    VwAccountProfile GetAccountProfile(string email);
    
    Task<VwAccountProfile> GetAccountProfileAsync(string email);
    
    bool DeleteAccount(short id);
}