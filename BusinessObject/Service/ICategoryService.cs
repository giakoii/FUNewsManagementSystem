using DataAccessObject.Models;

namespace BusinessObject.Service
{
    public interface ICategoryService : IBaseService<Category, short>
    {
        bool AddCategory(Category category);
        
        void UpdateCategory(Category category);
        
        bool DeleteCategory(short id);
        
        bool IsCategoryInUse(short categoryId);
        
        IEnumerable<Category> GetAllSubCategory(short categoryId);
    }
}
