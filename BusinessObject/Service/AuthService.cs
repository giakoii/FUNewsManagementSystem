using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccessObject.Models;
using DataAccessObject.Repositories;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Service
{
    public class AuthService : IAuthService
    {
        private readonly SystemAccountRepository _systemAccountRepository;
        private readonly IConfiguration _configuration;

        public AuthService(SystemAccountRepository systemAccountRepository, IConfiguration configuration)
        {
            _systemAccountRepository = systemAccountRepository;
            _configuration = configuration;
        }

        public async Task<SystemAccount> LoginAsync(string username, string password)
        {
            return _systemAccountRepository.GetAll(x => x.AccountEmail == username && x.AccountPassword == password, true).FirstOrDefault();
        }
        public async Task<SystemAccount> LoginAdmin(string username, string password)
        {
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];
            var role = _configuration["AdminAccount:Role"];
            int roleNum = 0;
            if (role == "Admin")
            {
                roleNum = 3;
            }

            if (username == adminEmail && password == adminPassword)
            {
                return new SystemAccount
                {
                    AccountName = adminEmail,
                    AccountEmail = adminEmail,
                    AccountPassword = adminPassword,
                    AccountRole = roleNum
                };
            }

            return null;
        }
        public SystemAccount GetUserById(short id)
        {
            return _systemAccountRepository.GetAll(
                x => x.AccountId == id,
                includes: x => x.NewsArticles
            ).FirstOrDefault();
        }

        public void UpdateUser(SystemAccount user)
        {
            _systemAccountRepository.Update(user);
        }
    }
}
