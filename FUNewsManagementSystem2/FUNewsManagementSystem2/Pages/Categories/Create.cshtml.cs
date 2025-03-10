using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Categories;


public class Create : PageModel
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="categoryService"></param>
    public Create(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [BindProperty]
    public AddCategoryVM CategoryModel { get; set; } = new();

    public List<CategoryModelViewModel> ParentCategories { get; set; } = new();

    /// <summary>
    /// Get parent categories
    /// </summary>
    public void OnGet()
    {
        ParentCategories = _categoryService.GetBy()
            .Select(c => new CategoryModelViewModel
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }).ToList();
    }

    /// <summary>
    /// Add new category
    /// </summary>
    /// <returns></returns>
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        var newCategory = new Category
        {
            CategoryName = CategoryModel.CategoryName,
            CategoryDesciption = CategoryModel.CategoryParentDesciption,
            IsActive = true,
            ParentCategoryId = CategoryModel.ParentCategoryId
        };

        _categoryService.AddCategory(newCategory);
        TempData["ToastMessage"] = "Category added successfully!";
        TempData["ToastType"] = "success";
        return RedirectToPage("Index");
    }
}