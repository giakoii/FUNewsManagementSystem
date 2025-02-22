using BusinessObject.SystemAccountService;
using DataAccessObject.Models;
using DataAccessObject.Repositories;

namespace BusinessObject.SystemAccountService;

/// <summary>
/// SystemAccountServiceImp
/// </summary>
public class SystemAccountServiceImp : ISystemAccountService
{
    private readonly ISystemAccountRepository _systemAccountRepository;

    public SystemAccountServiceImp(ISystemAccountRepository systemAccountRepository)
    {
        _systemAccountRepository = systemAccountRepository;
    }

    /// <summary>
    /// Select SystemAccount By Email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public VwAccountProfile GetAccountProfileByEmail(string email)
    {
        VwAccountProfile accountProfile;
        try
        {
            accountProfile = _systemAccountRepository.GetAccountProfile(email);
        }
        catch (Exception e)
        {
            throw new Exception();
        }

        return accountProfile;
    }

    /// <summary>
    /// Get Account By Email for Create - Update - Delete
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public SystemAccount GetAccountByEmail(string email)
    {
        SystemAccount systemAccount;
        try
        {
            systemAccount = _systemAccountRepository.GetAll(x => x.AccountEmail == email, false, null).FirstOrDefault();
        }
        catch (Exception e)
        {
            throw new Exception();
        }

        return systemAccount;
    }

    /// <summary>
    /// Get Account By Email for Create - Update - Delete
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<VwAccountProfile> GetAccountByEmailAsync(string email)
    {
        VwAccountProfile accountProfile;
        try
        {
            accountProfile = await _systemAccountRepository.GetAccountProfileAsync(email);
        }
        catch (Exception e)
        {
            throw new Exception();
        }

        return accountProfile;
    }

    /// <summary>
    /// Update Account
    /// </summary>
    /// <param name="account"></param>
    public void UpdateAccount(SystemAccount account)
    {
        try
        {
            _systemAccountRepository.Update(account);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Delete Account
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// Get News History
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<ViewUserNewsHistory> GetNewsHistory(string email)
    {
        List<ViewUserNewsHistory> newsHistories;
        try
        {
            newsHistories = _systemAccountRepository.GetNewsHistory(email);
        }
        catch (Exception e)
        {
            throw new Exception();
        }

        return newsHistories;
    }
}