using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObject.Models;

namespace BusinessObject.Service
{
    public interface IAuthService
    {
        Task<SystemAccount> LoginAsync(string username, string password);
        Task<SystemAccount> LoginAdmin(string username, string password);
        SystemAccount GetUserById(short id);
        void UpdateUser(SystemAccount user);
    }
}
