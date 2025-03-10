using DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Repositories
{
    public class TagsRepository : BaseRepository<Tag, int>, ITagRepository
    {
        public TagsRepository(FUNewsManagementSystemContext context) : base(context)
        {
        }
    }
}
