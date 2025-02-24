using DataAccessObject.Models;
using DataAccessObject.Repositories;

namespace BusinessObject.Service
{
    /// <summary>
    /// NewArticleService - Service for NewsArticle
    /// </summary>
    public class NewArticleService : BaseService<NewsArticle, string>, INewArticleService
    {
        private readonly ITagService _tagService;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="tagService"></param>
        public NewArticleService(BaseRepository<NewsArticle, string> repository, ITagService tagService) : base(repository)
        {
            _tagService = tagService;
        }
        
        /// <summary>
        /// Add new article
        /// </summary>
        /// <param name="newsArticle"></param>
        /// <returns></returns>
        public bool AddNewsArticle(NewsArticle newsArticle)
        {
            newsArticle.NewsArticleId = "ART-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            newsArticle.CreatedDate = DateTime.Now;
            newsArticle.NewsStatus = true;

            var tagIds = newsArticle.Tags.Select(t => t.TagId).ToList();
            var existingTags = _tagService.GetBy(x => tagIds.Contains(x.TagId));
            newsArticle.Tags.Clear();
            
            foreach (var tag in existingTags)
            {
                newsArticle.Tags.Add(tag);
            }
            Repository.Add(newsArticle);
            return true;
        }

        /// <summary>
        /// Delete article
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteNewsArticle(string id)
        {
            var article = Repository
                .GetBy(x => x.NewsArticleId.Equals(id), 
                    true,
                    a => a.Tags)
                .FirstOrDefault(a => a.NewsArticleId == id);

            if (article == null)
            {
                return false;
            }

            Repository.Delete(id);
            return true;
        }

        /// <summary>
        /// Update article
        /// </summary>
        /// <param name="newsArticle"></param>
        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            var existingArticle = Repository
                .GetBy(x => x.NewsArticleId == newsArticle.NewsArticleId, false, t => t.Tags)
                .FirstOrDefault();
                
            if (existingArticle != null)
            {
                existingArticle.NewsTitle = newsArticle.NewsTitle;
                existingArticle.Headline = newsArticle.Headline;
                existingArticle.NewsContent = newsArticle.NewsContent;
                existingArticle.NewsSource = newsArticle.NewsSource;
                existingArticle.CategoryId = newsArticle.CategoryId;
                existingArticle.ModifiedDate = DateTime.Now;

                // Clear existing tags
                existingArticle.Tags.Clear();
                foreach (var tag in newsArticle.Tags)
                {
                    existingArticle.Tags.Add(tag);
                }

                Repository.Add(existingArticle);
            }
        }
    }
}