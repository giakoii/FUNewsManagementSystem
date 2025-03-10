using DataAccessObject.Models;

namespace FUNewsManagementSystem.Models.ViewModel;

public class CategoryViewModel
{
    public List<CategoryModelViewModel> Categories { get; set; }
    public UpdateCategoryViewModel UpdateCategory { get; set; }
}