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

        public IQueryable<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.NewsArticles.Where(article => article.CreatedDate >= startDate && article.CreatedDate <= endDate)
                .OrderByDescending(article => article.CreatedDate);

        }
    }
}
