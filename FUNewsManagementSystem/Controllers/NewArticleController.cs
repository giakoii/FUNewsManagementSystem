using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    public class NewArticleController : Controller
    {
        private readonly INewArticleService _articleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;

        public NewArticleController(INewArticleService articleService, ITagService tagService,
            ICategoryService categoryService)
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
            var categories = _categoryService.GetBy();
            ViewBag.Categories = categories;
            ViewBag.Tags = _tagService.GetBy();
            if (User.IsInRole("Staff"))
            {
                return View(_articleService.GetBy());
            }

            var articles = _articleService.GetBy(x => x.NewsStatus == true);
            return View(articles);
        }

        [HttpPost]
        public IActionResult AddArticle(AddNewArticleRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetBy();
                ViewBag.Tags = _tagService.GetBy();
                return View("Index", _articleService.GetBy());
            }

            var newArticle = new NewsArticle
            {
                NewsTitle = model.NewsTitle,
                Headline = model.HeadLine,
                NewsSource = model.NewSource,
                NewsContent = model.NewsContent,
                CategoryId = (short?)model.SelectedCategory,
                CreatedById = GetCurrentUserId(),
            };
            if (model.SelectedTags != null && model.SelectedTags.Any())
            {
                foreach (var tagId in model.SelectedTags)
                {
                    var tag = _tagService.GetById(tagId);
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
                var existingArticle = _articleService.GetById(updatedArticle.NewsArticleId);

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

        [HttpPost]
        public IActionResult DeleteArticle(string newsArticleId)
        {
            var isDeleted = _articleService.DeleteNewsArticle(newsArticleId);
            if (isDeleted)
            {
                return Ok(new
                {
                    message = "Article deleted successfully."
                });
            }
            else
            {
                return NotFound(new { message = "Article not found." });
            }
        }

        private short GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? short.Parse(userIdClaim.Value) : (short)0;
        }
    }
}