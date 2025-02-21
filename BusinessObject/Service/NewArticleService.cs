using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObject.Models;
using DataAccessObject.Repositories;

namespace BusinessObject.Service
{
    public class NewArticleService : INewArticleService
    {
        private readonly NewArticleRepository _newArticleRepository;

        public NewArticleService(NewArticleRepository newArticleRepository)
        {
            _newArticleRepository = newArticleRepository;
        }

        public bool AddNewsArticle(NewsArticle newsArticle)
        {
            throw new NotImplementedException();
        }

        public bool DeleteNewsArticle(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NewsArticle> GetAllNewsArticles()
        {
            return _newArticleRepository.GetAll(
            null,
            true,
            x => x.Category,
            x => x.Tags
          ).ToList();
        }

        public NewsArticle GetNewsArticleById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            throw new NotImplementedException();
        }
    }
}
