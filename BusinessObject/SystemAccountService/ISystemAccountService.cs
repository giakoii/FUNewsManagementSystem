using DataAccessObject.Models;

namespace BusinessObject.SystemAccountService;

public interface ISystemAccountService
{
    VwAccountProfile GetAccountProfileByEmail(string email);
    
    SystemAccount GetAccountByEmail(string email); 

    Task<VwAccountProfile> GetAccountByEmailAsync(string email);
    
    void UpdateAccount(SystemAccount account);
    bool DeleteAccount(short id);
    
    List<ViewUserNewsHistory> GetNewsHistory(string email);
}