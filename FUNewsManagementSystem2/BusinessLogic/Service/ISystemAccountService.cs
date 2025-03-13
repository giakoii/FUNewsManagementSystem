using DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTOs;

namespace BusinessObject.Service
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
        
        public List<VwUserNewsHistoryDto> GetNewsHistory(short id);

        public SystemAccountDto GetAccountByEmail(string email);
    }
}
