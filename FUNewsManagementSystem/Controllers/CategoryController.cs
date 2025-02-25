using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Controllers;
/// <summary>
/// https://localhost:7047/Category
/// </summary>
[Authorize(Roles = "Staff")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="categoryService"></param>
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    /// <summary>
    /// Get Category
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        return View(_categoryService.GetBy(null, false, c => c.ParentCategory, c => c.InverseParentCategory));
    }
    /// <summary>
    /// Add new category
    /// </summary>
    /// <param name="modal"></param>
    /// <param name="isAddParent"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateCategory(AddCategoryVM modal, bool isAddParent = false)
    {
        short? parentCategoryId = null;
        if (isAddParent)
        {
            var parentCategory = new Category
            {
                CategoryName = modal.CategoryParentName,
                CategoryDesciption = modal.CategoryParentDesciption,
                IsActive = true,
                ParentCategoryId = null,
            };
            _categoryService.AddCategory(parentCategory);

            parentCategoryId = parentCategory.CategoryId;
        }
        else
        {
            parentCategoryId = modal.ParentCategoryId;
        }
        var childCategory = new Category
        {
            CategoryName = modal.CategoryName,
            CategoryDesciption = modal.CategoryDesciption,
            IsActive = true,
            ParentCategoryId = parentCategoryId
        };
        _categoryService.AddCategory(childCategory);
        TempData["ToastMessage"] = "Add New Category successfully!";
        TempData["ToastType"] = "success";
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult Delete(short id)
    {
        if (_categoryService.IsCategoryInUse(id))
        {
            TempData["Error"] = "Cannot delete category in use!";
            return RedirectToAction(nameof(Index));
        }

        _categoryService.DeleteCategory(id);
        return RedirectToAction(nameof(Index));
    }

}