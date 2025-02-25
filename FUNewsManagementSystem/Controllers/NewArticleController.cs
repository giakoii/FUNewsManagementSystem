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
        public string GetSearchTerm()
        {
            var searchTerm = HttpContext.Request.Query["searchTerm"].ToString();
            return searchTerm;
        }

        /// <summary>
        /// Get New Article
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpGet]
        public IActionResult Index(bool isTable = false, string sortBy = "Title", string sortOrder = "asc")
        {
            var searchTerm = GetSearchTerm();
            ViewBag.IsTable = isTable;
            var categories = _categoryService.GetBy().ToList();
            ViewBag.Categories = categories;
            ViewBag.Tags = _tagService.GetBy().ToList();

            var articles = _articleService.GetBy(
                x => (x.NewsStatus == true) && (x.NewsTitle.ToLower().Contains(searchTerm.ToLower())),
                true,
                a => a.Category,
                t => t.Tags
            );

            articles = sortBy switch
            {
                "Title" => (sortOrder == "asc") ? articles.OrderBy(a => a.NewsTitle) : articles.OrderByDescending(a => a.NewsTitle),
                "Id" => (sortOrder == "asc") ? articles.OrderBy(a => a.NewsArticleId) : articles.OrderByDescending(a => a.NewsArticleId),
                "Date" => (sortOrder == "asc") ? articles.OrderBy(a => a.CreatedDate) : articles.OrderByDescending(a => a.CreatedDate),
                _ => articles
            };

            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;

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
                NewsArticleId = GetNextNewsArticleId(),
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
        public IActionResult UpdateArticle(EditNewsArticleVM updatedArticle, List<int> SelectedTags)
        {
            var existingArticle = _articleService.GetById(updatedArticle.NewsArticleId);

            if (existingArticle != null)
            {
                existingArticle.NewsTitle = updatedArticle.NewsTitle;
                existingArticle.NewsContent = updatedArticle.NewsContent;
                existingArticle.CategoryId = (short?)updatedArticle.SelectedCategory;
                foreach (var tagId in SelectedTags)
                {
                    var tag = _tagService.GetById(tagId);
                    if (tag != null)
                    {
                        existingArticle.Tags.Add(tag);
                    }
                }
                _articleService.UpdateNewsArticle(existingArticle);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteArticle(string newsArticleId)
        {
            var isDeleted = _articleService.DeleteNewsArticle(newsArticleId);
            if (isDeleted)
            {
                TempData["ToastMessage"] = "Article deleted successfully!";
                TempData["ToastType"] = "success";
            }
            else
            {
                TempData["ToastMessage"] = "Failed to delete article.";
                TempData["ToastType"] = "danger";
            }

            return RedirectToAction("Index");
        }


        private short GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? short.Parse(userIdClaim.Value) : (short)0;
        }
        private string GetNextNewsArticleId()
        {
            var articles = _articleService.GetBy();

            int maxId = articles
                .ToList()
                .Max(a => int.Parse(a.NewsArticleId));

            return (maxId + 1).ToString();
        }
    }
}