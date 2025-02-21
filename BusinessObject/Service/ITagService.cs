using DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public interface ITagService
    {
        IEnumerable<NewsArticle> GetAllNewsArticles();
        NewsArticle GetNewsArticleById(int id);
        bool AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        bool DeleteNewsArticle(int id);
    }
}
