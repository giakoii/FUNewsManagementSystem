using DataAccessObject.Models;
using DataAccessObject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository _accountRepository;
        public AccountService(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<bool> CreateSystemAccountAsync(SystemAccount account)
        {
            try
            {
                var newAccount = new SystemAccount
                {
                    AccountId = _accountRepository.GetNextAccountId(),
                    AccountName = account.AccountName,
                    AccountEmail = account.AccountEmail,
                    AccountPassword = account.AccountPassword,
                    AccountRole = account.AccountRole,
                };
                var res = _accountRepository.Add(newAccount);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Creating new user" + ex.Message);
            }
        }

        public async Task<bool> DeleteSystemAccountAsync(short id)
        {
            try
            {
               return await _accountRepository.DeleteSystemAccountAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Deleting user" + ex.Message);
            }
        }

        public async Task<SystemAccount> GetSystemAccountByIdAsync(int id)
        {
            try
            {
                var account = _accountRepository.GetById(id);
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Fetching user" + ex.Message);
            }
        }

        public async Task<List<SystemAccount>> GetSystemAccountsAsync()
        {
            try
            {
                var listAccount = _accountRepository.GetAll().ToList();
                return listAccount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Fetching user" + ex.Message);
            }
        }

        public async Task UpdateSystemAccountAsync(SystemAccount account)
        {
            try
            {
                _accountRepository.Update(account);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Updating user" + ex.Message);
            }
        }
    }
}
