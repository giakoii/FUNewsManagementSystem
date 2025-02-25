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
        var categories = _categoryService.GetBy(null, false, c => c.ParentCategory, c => c.InverseParentCategory)
            .AsEnumerable() // Chuyển về IEnumerable để tránh lỗi
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

        var viewModel = new CategoryViewModel
        {
            Categories = categories
        };

        return View(viewModel);
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
        TempData["Success"] = "Category deleted successfully!";
        return RedirectToAction(nameof(Index));
    }


    /// <summary>
    /// Update category
    /// </summary>
    /// <param name="modal"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult UpdateCategory(UpdateCategoryViewModel modal)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            TempData["Error"] = string.Join("; ", errors);
            return RedirectToAction("Index");
        }
        var category = _categoryService.GetById(modal.CategoryId);
        if (category != null)
        {
            category.CategoryName = modal.CategoryName;
            category.CategoryDesciption = modal.CategoryDescription;
            category.ParentCategoryId = modal.ParentCategoryId;
            category.IsActive = modal.IsActive;
            _categoryService.UpdateCategory(category);
            TempData["ToastMessage"] = "Update Category successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Invalid data!";
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Delete category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult DeleteCategory(short id)
    {
        if (_categoryService.IsCategoryInUse(id))
        {
            TempData["Error"] = "Cannot delete category in use!";
            return RedirectToAction(nameof(Index));
        }

        _categoryService.DeleteCategory(id);
        TempData["ToastMessage"] = "Delete Category successfully!";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Index));
    }
}