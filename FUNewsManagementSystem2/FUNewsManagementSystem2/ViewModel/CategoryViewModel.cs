using DataAccessObject.Models;

namespace FUNewsManagementSystem.Models.ViewModel;

public class CategoryViewModel
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryDescription { get; set; }
    public bool? IsActive { get; set; }
    
    public short? ParentCategoryId { get; set; }
    
    public List<CategoryModelViewModel>? SubCategories { get; set; }
}