using BusinessObject.Service;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Categories;

/// <summary>
/// 
/// </summary>
public class Edit : PageModel
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="categoryService"></param>
    public Edit(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [BindProperty]
    public UpdateCategoryViewModel CategoryModel { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult OnGet(short id)
    {
        var category = _categoryService.GetById(id);
        if (category == null) return RedirectToPage("Index");

        CategoryModel = new UpdateCategoryViewModel
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            CategoryDescription = category.CategoryDesciption
        };

        return Page();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        // Get category
        var category = _categoryService.GetById(CategoryModel.CategoryId);
        if (category != null)
        {
            category.CategoryName = CategoryModel.CategoryName;
            category.CategoryDesciption = CategoryModel.CategoryDescription;
            _categoryService.UpdateCategory(category);
            TempData["ToastMessage"] = "Category updated successfully!";
            TempData["ToastType"] = "success";
        }

        return RedirectToPage("Index");
    }
}