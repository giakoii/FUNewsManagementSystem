using BusinessLogic.DTOs;
using DataAccessObject.Models;

namespace BusinessObject.Service
{
    public interface INewArticleService : IBaseService<NewsArticle, string>
    {
        bool AddNewsArticle(NewsArticle newsArticle);
        
        List<NewsArticleDto> GetNewsArticles();
        
        bool DeleteNewsArticle(string id);

        void UpdateNewsArticle(NewsArticle newsArticle);
        List<NewsArticleDto> GetNewsReportByDateRange(DateTime startDate, DateTime endDate);
    }
}
