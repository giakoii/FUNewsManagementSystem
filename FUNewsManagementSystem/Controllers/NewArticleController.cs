using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    public class NewArticleController : Controller
    {
        private readonly INewArticleService _articleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;

        public NewArticleController(INewArticleService articleService, ITagService tagService, ICategoryService categoryService)
        {
            _articleService = articleService;
            _tagService = tagService;
            _categoryService = categoryService;
        }
        /// <summary>
        /// Get New Article
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var tags = _tagService.GetAllTag();
            var categories = _categoryService.GetAllCategory();
            ViewBag.Categories = categories;
            ViewBag.Tags = _tagService.GetAllTag();
            if (User.IsInRole("Staff"))
            {
                return View(_articleService.GetAllNewsArticles());
            }
            var articles = _articleService.GetAllNewsArticles().Where(x => x.NewsStatus == true);
            return View(articles);
        }
        [HttpPost]
        public IActionResult AddArticle(AddNewArticleRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetAllCategory();
                ViewBag.Tags = _tagService.GetAllTag();
                return View("Index", _articleService.GetAllNewsArticles());
            }

            var newArticle = new NewsArticle
            {
                NewsArticleId = GetNextNewsArticleId(),
                NewsTitle = model.NewsTitle,
                Headline = model.HeadLine,
                NewsSource = model.NewSource,
                NewsContent = model.NewsContent,
                CategoryId = (short?)model.SelectedCategory,
                CreatedDate = DateTime.Now,
                NewsStatus = true,
                CreatedById = GetCurrentUserId(),
            };
            if (model.SelectedTags != null && model.SelectedTags.Any())
            {
                foreach (var tagId in model.SelectedTags)
                {
                    var tag = _tagService.GetTagById(tagId);
                    if (tag != null)
                    {
                        newArticle.Tags.Add(tag);
                    }
                }
            }
            _articleService.AddNewsArticle(newArticle);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateArticle(NewsArticle updatedArticle, List<int> SelectedTags)
        {
            if (ModelState.IsValid)
            {
                var existingArticle = _articleService.GetNewsArticleById(int.Parse(updatedArticle.NewsArticleId));

                if (existingArticle != null)
                {
                    existingArticle.NewsTitle = updatedArticle.NewsTitle;
                    existingArticle.NewsContent = updatedArticle.NewsContent;

                    _articleService.UpdateNewsArticle(existingArticle);

                    return RedirectToAction("Index");
                }
            }

            Console.WriteLine("ModelState không hợp lệ hoặc bài viết không tìm thấy.");
            return View(updatedArticle);
        }

        private short GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? short.Parse(userIdClaim.Value) : (short)0;
        }
        private string GetNextNewsArticleId()
        {
            // Lấy danh sách tất cả bài viết
            var articles = _articleService.GetAllNewsArticles();

            // Chuyển đổi các NewsArticleID sang số nguyên (nếu không chuyển được thì coi là 0)
            int maxId = articles
                .Select(a => int.TryParse(a.NewsArticleId, out int num) ? num : 0)
                .DefaultIfEmpty()
                .Max();

            // Tăng giá trị lên 1 và chuyển về chuỗi
            return (maxId + 1).ToString();
        }

    }
}
