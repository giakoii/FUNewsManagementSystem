namespace FUNewsManagementSystem.Models.ViewModel;

public class CategoryModelViewModel
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryDescription { get; set; }
    public bool? IsActive { get; set; }
    
    public CategoryModelViewModel? ParentCategory { get; set; }
}