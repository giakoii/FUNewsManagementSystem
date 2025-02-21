using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObject.Models;

namespace BusinessObject.Service
{
    public interface INewArticleService
    {
        IEnumerable<NewsArticle> GetAllNewsArticles();
        NewsArticle GetNewsArticleById(int id);
        bool AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        bool DeleteNewsArticle(int id);
    }
}
