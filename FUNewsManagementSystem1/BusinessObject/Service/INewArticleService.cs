using DataAccessObject.Models;

namespace BusinessObject.Service
{
    public interface INewArticleService : IBaseService<NewsArticle, string>
    {
        bool AddNewsArticle(NewsArticle newsArticle);
        
        bool DeleteNewsArticle(string id);

        void UpdateNewsArticle(NewsArticle newsArticle);
        List<NewsArticle> GetNewsReportByDateRange(DateTime startDate, DateTime endDate);
    }
}
