using FUNewsManagementSystem.Models.ViewModel;

namespace FUNewsManagementSystem2.ViewModel;

public class CategoryViewModel
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryDesciption { get; set; }
    public bool? IsActive { get; set; }
    
    public short? ParentCategoryId { get; set; }
    
    public List<CategoryModelViewModel>? SubCategories { get; set; }
}