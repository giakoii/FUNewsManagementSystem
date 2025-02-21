using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObject.Models;

namespace DataAccessObject.Repositories
{
    public class SystemAccountRepository : BaseRepository<SystemAccount>
    {
        public SystemAccountRepository(FUNewsManagementSystemContext context) : base(context)
        {
        }
    }
}
