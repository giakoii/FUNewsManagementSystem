using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Service;
using DataAccessObject.Models;
using DataAccessObject.Repositories;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Service;

/// <summary>
/// AccountService - Service class for system account
/// </summary>
public class SystemAccountService : BaseService<SystemAccount, short>, ISystemAccountService
{
    private readonly ISystemAccountRepository _systemAccountRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="configuration"></param>
    /// <param name="systemAccountRepository"></param>
    /// <param name="mapper"></param>
    public SystemAccountService(BaseRepository<SystemAccount, short> repository, IConfiguration configuration, ISystemAccountRepository systemAccountRepository, IMapper mapper) : base(repository)
    {
        _configuration = configuration;
        _systemAccountRepository = systemAccountRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Create new system account
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> CreateSystemAccountAsync(SystemAccount account)
    {
            var newAccount = new SystemAccount
            {
                AccountId = (short) (Repository.Table.Max(x => x.AccountId) + 1),
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountPassword = account.AccountPassword,
                AccountRole = account.AccountRole,
            };
            var res = Repository.Add(newAccount);
            return await Task.FromResult(res);
    }

    /// <summary>
    /// Delete system account
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> DeleteSystemAccountAsync(short id)
    {
        var userExist = await GetByIdAsync(id);
        if (userExist != null)
        {
            Repository.Delete(id);
            return true;
        }
        return false;
    }

    public async Task<List<SystemAccountDto>> GetSystemAccountsAsync()
    {
        var accounts = await Repository.GetByAsync();
    
        return _mapper.Map<List<SystemAccountDto>>(accounts.ToList());
    }


    /// <summary>
    /// Update system account
    /// </summary>
    /// <param name="account"></param>
    public Task UpdateSystemAccountAsync(SystemAccount account)
    {
        Repository.Update(account);
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Login
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<SystemAccount?> LoginAsync(string email, string password)
    {
        var userQuery = await Repository.GetByAsync(x => x.AccountEmail == email && x.AccountPassword == password, true);
        var user = userQuery.FirstOrDefault();
        return user;
    }

    /// <summary>
    /// Login as admin
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
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

    public void UpdateUser(SystemAccount user)
    {
        try
        {
            var userExist = Repository.GetById(user.AccountId);

            if (userExist != null)
            {
                userExist.AccountName = user.AccountName;
                Repository.Update(user);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    
    /// <summary>
    /// Delete account
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool DeleteAccount(short id)
    {
        try
        {
            return _systemAccountRepository.DeleteAccount(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Get news history create News Actice of user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<VwUserNewsHistoryDto> GetNewsHistory(short id)
    {
        var newsHistory = _systemAccountRepository.GetNewsHistoriesByAccountId(id);
        return _mapper.Map<List<VwUserNewsHistoryDto>>(newsHistory);
    }

    public SystemAccountDto GetAccountByEmail(string email)
    {
        return  _mapper.Map<SystemAccountDto>(_systemAccountRepository.GetAccountByEmail(email));
    }

    public List<VwUserNewsHistoryDto> GetNewsHistoryByAccountId(short id)
    {
        var newsHistory = _systemAccountRepository.GetNewsHistoriesByAccountId(id);
        
        return _mapper.Map<List<VwUserNewsHistoryDto>>(newsHistory);
    }

    public bool AddSystemAccount(SystemAccountDto account)
    {
        var newAccount = _mapper.Map<SystemAccount>(account);
        var accCount = Repository.GetBy().ToList().Max(a => a.AccountId);
        short accountId = (short) (accCount + 1);
        newAccount.AccountId = accountId;
        newAccount.AccountEmail = account.AccountEmail;
        newAccount.AccountName = account.AccountName;
        newAccount.AccountRole = account.AccountRole;
        Repository.Add(newAccount);
        return true;
    }
}
