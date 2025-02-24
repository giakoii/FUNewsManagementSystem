using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccessObject.Models;

namespace DataAccessObject.Repositories
{
    public class NewArticleRepository : BaseRepository<NewsArticle, string>, INewArticelRepository
    {
        public NewArticleRepository(FUNewsManagementSystemContext context) : base(context)
        {
        }
    }
}
