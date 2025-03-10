namespace BusinessLogic.DTOs;

public class CategoryDto
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryDescription { get; set; }
    public short? ParentCategoryId { get; set; }
    public bool? IsActive { get; set; }
}