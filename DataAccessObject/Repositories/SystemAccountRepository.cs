using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject.Repositories;

public class SystemAccountRepository : BaseRepository<SystemAccount>, ISystemAccountRepository
{
    public SystemAccountRepository(FUNewsManagementSystemContext context) : base(context)
    {
    }

    public VwAccountProfile GetAccountProfile(string email)
    {
        return _context.VwAccountProfiles.AsNoTracking().FirstOrDefault(x => x.AccountEmail == email);
    }

    public async Task<VwAccountProfile> GetAccountProfileAsync(string email)
    {
        return await _context.VwAccountProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.AccountEmail == email);
    }

    public bool DeleteAccount(short id)
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}
