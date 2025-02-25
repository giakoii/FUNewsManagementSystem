using DataAccessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject.Repositories;

public class SystemAccountRepository : BaseRepository<SystemAccount, short>, ISystemAccountRepository
{
    public SystemAccountRepository(FUNewsManagementSystemContext context) : base(context)
    {
    }

    public bool DeleteAccount(short id)
    {
        // Find Account
        var account = _context.SystemAccounts
            .Include(x => x.NewsArticles)
            .ThenInclude(n => n.Tags) // Include Tags của NewsArticle
            .FirstOrDefault(x => x.AccountId == id);

        if (account == null)
        {
            return false;
        }

        // Delete relationship
        foreach (var article in account.NewsArticles)
        {
            article.Tags.Clear();
        }

        // Delete NewsArticle
        _context.NewsArticles.RemoveRange(account.NewsArticles);

        // Delete Account
        _context.SystemAccounts.Remove(account);

        // Save change
        _context.SaveChanges();
        return true;
    }

    /// <summary>
    /// Get news history create News Actice of user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ViewUserNewsHistory> GetNewsHistory(short id)
    {
        var result = _context.ViewUserNewsHistories.
            AsNoTracking()
            .Where(x => x.CreatedById == id)
            .ToList();
        
        return result;
    }
    
    /// <summary>
    /// Get account by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public SystemAccount GetAccountByEmail(string email)
    {
        return _context.SystemAccounts.FirstOrDefault(x => x.AccountEmail == email);
    }
}