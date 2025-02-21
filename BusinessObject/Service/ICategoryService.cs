using DataAccessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(short id);
        bool AddCategory(Category category);
        void UpdateCategory(Category category);
        bool DeleteCategory(short id);
        bool IsCategoryInUse(short categoryId);
    }
}
