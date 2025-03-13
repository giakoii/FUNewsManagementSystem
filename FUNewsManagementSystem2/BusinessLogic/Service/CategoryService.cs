using AutoMapper;
using BusinessLogic.DTOs;
using DataAccessObject.Models;
using DataAccessObject.Repositories;

namespace BusinessObject.Service
{
    public class CategoryService : BaseService<Category, short>, ICategoryService
    {
        private readonly INewArticleService _newArticleService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="newArticleService"></param>
        /// <param name="mapper"></param>
        public CategoryService(BaseRepository<Category, short> repository, INewArticleService newArticleService, IMapper mapper) : base(repository)
        {
            _newArticleService = newArticleService;
            _mapper = mapper;
        }

        public List<CategoryDto> GetBy()
        {
            var categories = GetBy(x => x.IsActive == true, false).ToList();

            return _mapper.Map<List<CategoryDto>>(categories.ToList());
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
            try
            {
                var newArticle = _newArticleService.GetBy(x => x.CategoryId == id).FirstOrDefault();
                var categorySelect = Repository.GetById(id);
                if (newArticle != null || categorySelect == null)
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
        public List<CategoryDto> GetAllSubCategory(short categoryId)
        {
            var subCategories = GetBy(x => x.ParentCategoryId == categoryId, false, null);
            return _mapper.Map<List<CategoryDto>>(subCategories);
        }
    }
}
