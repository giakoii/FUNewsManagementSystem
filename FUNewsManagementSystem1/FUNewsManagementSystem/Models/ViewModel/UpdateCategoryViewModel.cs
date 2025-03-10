namespace FUNewsManagementSystem.Models.ViewModel;

public class UpdateCategoryViewModel
{
    public string CategoryName { get; set; }
    
    public string CategoryDescription { get; set; }
    
    public short CategoryId { get; set; }
    
    public short? ParentCategoryId { get; set; }
    
    public bool IsActive { get; set; }
    
}