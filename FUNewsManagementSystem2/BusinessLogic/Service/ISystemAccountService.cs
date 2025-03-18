using BusinessLogic.DTOs;
using BusinessObject.Service;
using DataAccessObject.Models;

namespace BusinessLogic.Service
{
    public interface ISystemAccountService : IBaseService<SystemAccount, short>
    {
        Task<SystemAccount?> LoginAsync(string email, string password);

        Task<SystemAccount> LoginAdmin(string username, string password);

        void UpdateUser(SystemAccount user);

        Task<List<SystemAccountDto>> GetSystemAccountsAsync();

        Task UpdateSystemAccountAsync(SystemAccount account);

        Task<bool> DeleteSystemAccountAsync(short id);

        Task<bool> CreateSystemAccountAsync(SystemAccount account);

        List<VwUserNewsHistoryDto> GetNewsHistory(short id);

        SystemAccountDto GetAccountByEmail(string email);

        List<VwUserNewsHistoryDto> GetNewsHistoryByAccountId(short id);
        bool AddSystemAccount(SystemAccountDto account);
        List<SystemAccount> GetAllUsers();

    }
}