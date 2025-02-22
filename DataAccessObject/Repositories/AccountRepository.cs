using DataAccessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Repositories
{
    public class AccountRepository : BaseRepository<SystemAccount>
    {
        private readonly FUNewsManagementSystemContext _context;
        public AccountRepository(FUNewsManagementSystemContext context) : base(context)
        {
            _context = context;
        }
        public short GetNextAccountId()
        {
            short lastId = (short)(_context.SystemAccounts.OrderByDescending(a => a.AccountId).Select(a => a.AccountId).FirstOrDefault() + 1);
            return lastId;
        }
        public async Task<bool> DeleteSystemAccountAsync(short id)
        {
            var user = await _context.SystemAccounts.FindAsync(id);
            if (user == null)
                throw new Exception("User not found.");
            _context.SystemAccounts.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
