using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObject.Models;
using DataAccessObject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject.Service
{
    public class NewArticleService : INewArticleService
    {
        private readonly NewArticleRepository _newArticleRepository;
        private readonly FUNewsManagementSystemContext _context;
        public NewArticleService(NewArticleRepository newArticleRepository, FUNewsManagementSystemContext context)
        {
            _newArticleRepository = newArticleRepository;
            _context = context;
        }

        public bool AddNewsArticle(NewsArticle newsArticle)
        {

            try
            {
                // Generate unique ID
                newsArticle.NewsArticleId = GenerateArticleId();

                // Set default values
                newsArticle.CreatedDate = DateTime.Now;
                newsArticle.NewsStatus = true;

                // Attach existing tags to prevent duplication
                var existingTags = newsArticle.Tags.ToList();
                newsArticle.Tags.Clear();

                foreach (var tag in existingTags)
                {
                    var existingTag = _context.Tags.Find(tag.TagId);
                    if (existingTag != null)
                    {
                        newsArticle.Tags.Add(existingTag);
                    }
                }

                _context.NewsArticles.Add(newsArticle);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

            return _newArticleRepository.Add(newsArticle);

        }

        private string GenerateArticleId()
        {
            return "ART-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        public bool DeleteNewsArticle(string id)
        {
            try
            {
                var article = _context.NewsArticles
                    .Include(a => a.Tags)
                    .FirstOrDefault(a => a.NewsArticleId == id);

                if (article == null)
                {
                    return false;
                }

                _context.NewsArticles.Remove(article);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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

        public NewsArticle GetNewsArticleById(string id)
        {

            return _newArticleRepository.GetAll(
                x => x.NewsArticleId == id,
                true, // isTracking
                x => x.Category,
                x => x.Tags
            ).FirstOrDefault();

            return _newArticleRepository.GetById(id);
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            try
            {
                var existingArticle = _context.NewsArticles
                    .Include(a => a.Tags)
                    .FirstOrDefault(a => a.NewsArticleId == newsArticle.NewsArticleId);

                if (existingArticle != null)
                {
                    existingArticle.NewsTitle = newsArticle.NewsTitle;
                    existingArticle.Headline = newsArticle.Headline;
                    existingArticle.NewsContent = newsArticle.NewsContent;
                    existingArticle.NewsSource = newsArticle.NewsSource;
                    existingArticle.CategoryId = newsArticle.CategoryId;
                    existingArticle.ModifiedDate = DateTime.Now;

                    // Cập nhật tags
                    existingArticle.Tags.Clear();
                    foreach (var tag in newsArticle.Tags)
                    {
                        existingArticle.Tags.Add(tag);
                    }

                    _context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
