using DataAccessObject.Models;

namespace DataAccessObject.Repositories;

public interface INewArticelRepository : IRepository<NewsArticle, string>
{
    public IQueryable<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate);

}