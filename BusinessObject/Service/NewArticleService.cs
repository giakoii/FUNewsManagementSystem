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
        private readonly INewArticelRepository _newsArticleRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="tagService"></param>
        public NewArticleService(BaseRepository<NewsArticle, string> repository, ITagService tagService, INewArticelRepository newArticelRepository) : base(repository)
        {
            _tagService = tagService;
            _newsArticleRepository = newArticelRepository;
        }

        /// <summary>
        /// Add new article
        /// </summary>
        /// <param name="newsArticle"></param>
        /// <returns></returns>
        public bool AddNewsArticle(NewsArticle newsArticle)
        {
            newsArticle.CreatedDate = DateTime.Now;
            newsArticle.NewsStatus = true;
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
            article.Tags.Clear();
            Repository.Delete(article);
            return true;
        }

        /// <summary>
        /// Update article
        /// </summary>
        /// <param name="newsArticle"></param>
        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            var newTags = newsArticle.Tags.ToList();
            var existingArticle = Repository
                .GetBy(x => x.NewsArticleId == newsArticle.NewsArticleId, true, a => a.Tags)
                .FirstOrDefault();

            if (existingArticle != null)
            {
                existingArticle.NewsTitle = newsArticle.NewsTitle;
                existingArticle.Headline = newsArticle.Headline;
                existingArticle.NewsStatus = newsArticle.NewsStatus;
                existingArticle.NewsContent = newsArticle.NewsContent;
                existingArticle.NewsSource = newsArticle.NewsSource;
                existingArticle.CategoryId = newsArticle.CategoryId;
                existingArticle.ModifiedDate = DateTime.Now;
                existingArticle.Tags.Clear();
                foreach (var tag in newTags)
                {
                    existingArticle.Tags.Add(tag);
                }
                Repository.Update(existingArticle);
            }
        }

        public List<NewsArticle> GetNewsReportByDateRange(DateTime startDate, DateTime endDate)
        {
            return _newsArticleRepository.GetNewsByDateRange(startDate, endDate).ToList();

        }
    }
}