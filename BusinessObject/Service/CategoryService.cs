using DataAccessObject.Models;
using DataAccessObject.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly FUNewsManagementSystemContext _context;
        public CategoryService(IRepository<Category> categoryRepository, FUNewsManagementSystemContext context)
        {
            _categoryRepository = categoryRepository;
            _context=context;
        }

        public bool AddCategory(Category category) => _categoryRepository.Add(category);
        public void UpdateCategory(Category category) => _categoryRepository.Update(category);
        public bool DeleteCategory(short id)
        {
            // Check if category is used in any article
            var isUsed = _context.NewsArticles
                .Any(a => a.CategoryId == id);

            if (isUsed)
            {
                return false;
            }

            return _categoryRepository.Delete(id);
        }


        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll() ?? Enumerable.Empty<Category>();
        }

        public Category GetCategoryById(short id) => _categoryRepository.GetById(id);

        public bool IsCategoryInUse(short categoryId) =>
            _categoryRepository.GetAll(x => x.CategoryId == categoryId && x.NewsArticles.Any()).Any();
    }
}
