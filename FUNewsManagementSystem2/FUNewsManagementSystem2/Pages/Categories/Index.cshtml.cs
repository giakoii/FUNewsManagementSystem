using BusinessObject.Enum;
using BusinessObject.Service;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Categories;

[Authorize(Roles = ConstRole.Staff)]
public class Index : PageModel
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="categoryService"></param>
    public Index(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public List<CategoryModelViewModel> Categories { get; set; } = new();
    public string? ToastMessage => TempData["ToastMessage"]?.ToString();
    public string? ToastType => TempData["ToastType"]?.ToString();

    /// <summary>
    /// Get categories
    /// </summary>
    public void OnGet()
    {
        Categories = _categoryService.GetBy()
            .Select(c => new CategoryModelViewModel
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDescription = c.CategoryDesciption,
                IsActive = c.IsActive,
                ParentCategory = c.ParentCategory != null ? new CategoryModelViewModel
                {
                    CategoryId = c.ParentCategory.CategoryId,
                    CategoryName = c.ParentCategory.CategoryName
                } : null
            }).ToList();
    }

    /// <summary>
    /// Delete category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IActionResult OnPostDelete(short id)
    {
        if (_categoryService.IsCategoryInUse(id))
        {
            TempData["ToastMessage"] = "Cannot delete category in use!";
            TempData["ToastType"] = "danger";
            return RedirectToPage();
        }

        _categoryService.DeleteCategory(id);
        TempData["ToastMessage"] = "Category deleted successfully!";
        TempData["ToastType"] = "success";
        return RedirectToPage();
    }
}