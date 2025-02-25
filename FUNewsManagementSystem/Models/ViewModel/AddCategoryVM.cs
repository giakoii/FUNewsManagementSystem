using DataAccessObject.Models;

namespace FUNewsManagementSystem.Models.ViewModel
{
    public class AddCategoryVM
    {
        public string CategoryName { get; set; } = null!;
        public string CategoryParentName { get; set; } = null!;
        public string CategoryDesciption { get; set; } = null!;
        public string CategoryParentDesciption { get; set; } = null!;
        public short? ParentCategoryId { get; set; }
        public virtual Category? ParentCategory { get; set; }
    }
}
