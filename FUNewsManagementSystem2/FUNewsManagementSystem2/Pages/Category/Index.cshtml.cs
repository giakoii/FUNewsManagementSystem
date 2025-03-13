using AutoMapper;
using BusinessLogic.DTOs;
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
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="categoryService"></param>
    /// <param name="mapper"></param>
    public Index(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [BindProperty] public List<CategoryViewModel> Categories { get; set; }

    [BindProperty] public CategoryViewModel CategoryView { get; set; }
    public string? ToastMessage => TempData["ToastMessage"]?.ToString();
    public string? ToastType => TempData["ToastType"]?.ToString();

    /// <summary>
    /// Get categories
    /// </summary>
    public void OnGet()
    {
        var categories = _categoryService.GetBy();
        Categories = _mapper.Map<List<CategoryViewModel>>(categories);
    }

    /// <summary>
    /// Create category
    /// </summary>
    /// <returns></returns>
    public IActionResult OnPostCreateCategory()
    {
        short? parentCategoryId = null;
        if (CategoryView.ParentCategoryId != null)
        {
            var parentId = _categoryService.GetById(CategoryView.ParentCategoryId.Value)?.CategoryId;
            TempData["ToastMessage"] = "ParentId category not exist!";
            parentId = parentCategoryId;
        }

        var newCategory = new DataAccessObject.Models.Category
        {
            CategoryName = CategoryView.CategoryName,
            CategoryDesciption = CategoryView.CategoryDescription,
            IsActive = true,
            ParentCategoryId = parentCategoryId,
        };
        _categoryService.AddCategory(newCategory);
        parentCategoryId = newCategory.CategoryId;

        TempData["ToastMessage"] = "Add New Category successfully!";
        TempData["ToastType"] = "success";
        
        var categories = _categoryService.GetBy();
        Categories = _mapper.Map<List<CategoryViewModel>>(categories);

        return RedirectToPage();
    }
    
    public IActionResult OnPostUpdateCategory()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var category = _categoryService.GetById(CategoryView.CategoryId);
        if (category == null)
        {
            return Page();
        }
        
        category.CategoryName = CategoryView.CategoryName;
        category.CategoryDesciption = CategoryView.CategoryDescription;
        category.IsActive = CategoryView.IsActive;
        category.ParentCategoryId = CategoryView.ParentCategoryId;
        _categoryService.UpdateCategory(category);
        
        TempData["ToastMessage"] = "Category updated successfully!";
        TempData["ToastType"] = "success";

        return RedirectToPage();
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