using BusinessObject.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View(_categoryService.GetAllCategories());
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

        // Thêm các action Create, Edit sử dụng modal
    }
}
