using DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Repositories
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(FUNewsManagementSystemContext context) : base(context)
        {
        }
    }
}
