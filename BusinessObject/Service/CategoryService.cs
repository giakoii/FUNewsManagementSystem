using DataAccessObject.Models;
using DataAccessObject.Repositories;

namespace BusinessObject.Service
{
    public class CategoryService : BaseService<Category, short>, ICategoryService
    {
        private readonly INewArticleService _newArticleService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="newArticleService"></param>
        public CategoryService(BaseRepository<Category, short> repository, INewArticleService newArticleService) : base(repository)
        {
            _newArticleService = newArticleService;
        }

        /// <summary>
        /// Add new category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool AddCategory(Category category)
        {
            return Repository.Add(category);
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="category"></param>
        public void UpdateCategory(Category category)
        {
            Repository.Update(category);
        }
        
        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCategory(short id)
        {
            // Check if category is used in any article
            var isUsed = GetBy(x => x.CategoryId == id);

            if (isUsed.Any())
            {
                return false;
            }

            return Repository.Delete(id);
        }

        /// <summary>
        /// Check if category is used in any article
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool IsCategoryInUse(short categoryId)
        {
            return GetBy(x => x.CategoryId == categoryId, 
                false,
                x => x.NewsArticles).Any();
        }

        /// <summary>
        /// Get all sub category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IEnumerable<Category> GetAllSubCategory(short categoryId)
        {
            return GetBy(x => x.ParentCategoryId == categoryId, false, null);
        }
    }
}
