using DataAccessObject.Models;
using DataAccessObject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetAllCategory()
        {
            return _categoryRepository.GetAll();
        }

        public IEnumerable<Category> GetAllSubCategory(int categoryID)
        {
            return _categoryRepository.GetAll(x => x.ParentCategoryId == categoryID, true, null);
        }
    }
}
