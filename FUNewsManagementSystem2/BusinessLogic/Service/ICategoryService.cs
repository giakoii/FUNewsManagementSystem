using BusinessLogic.DTOs;
using DataAccessObject.Models;

namespace BusinessObject.Service
{
    public interface ICategoryService : IBaseService<Category, short>
    {
        List<CategoryDto> GetBy();
        bool AddCategory(Category category);
        
        void UpdateCategory(Category category);
        
        bool DeleteCategory(short id);
        
        bool IsCategoryInUse(short categoryId);
        
        List<CategoryDto> GetAllSubCategory(short categoryId);
    }
}
