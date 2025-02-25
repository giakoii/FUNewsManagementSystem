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
            if (category.CategoryId == category.ParentCategoryId)
            {
                throw new Exception("A category cannot be its own parent.");
            }

            // Nếu có danh mục cha, cập nhật danh mục cha trước
            if (category.ParentCategoryId != null)
            {
                var parentCategory = Repository.GetById(category.ParentCategoryId ?? 0);
                if (parentCategory == null)
                {
                    throw new Exception("Parent category does not exist.");
                }

                // Cập nhật danh mục cha trước
                Repository.Update(parentCategory);
            }

            // Sau đó cập nhật danh mục hiện tại
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
            try
            {
                var categorySelect = GetById(id);
                if (categorySelect == null)
                {
                    return false;
                }
                return Repository.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Check if category is used in any article
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool IsCategoryInUse(short categoryId)
        {
            return GetBy(x => x.CategoryId == categoryId && x.NewsArticles.Any(), 
                false).Any();
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
