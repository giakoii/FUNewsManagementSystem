using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Hubs;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    /// <summary>
    /// https://localhost:7047/NewArticle
    /// </summary>
    public class NewArticleController : Controller
    {
        private readonly INewArticleService _articleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly IHubContext<NewsHub> _hubContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="articleService"></param>
        /// <param name="tagService"></param>
        /// <param name="categoryService"></param>
        public NewArticleController(INewArticleService articleService, ITagService tagService,
            ICategoryService categoryService, IHubContext<NewsHub> hubContext)
        {
            _articleService = articleService;
            _tagService = tagService;
            _categoryService = categoryService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Get New Article
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(bool isTable = false, string sortBy = "Title", string sortOrder = "asc")
        {
            var searchTerm = GetSearchTerm();
            ViewBag.IsTable = isTable;
            var categories = _categoryService.GetBy().ToList();
            ViewBag.Categories = categories;
            ViewBag.Tags = _tagService.GetBy().ToList();

            IEnumerable<NewsArticle> articles = Enumerable.Empty<NewsArticle>();

            if (!User.Identity.IsAuthenticated || User.IsInRole("Lecturer"))
            {
                articles = _articleService.GetBy(
                    x => (x.NewsStatus == true) && (x.NewsTitle.ToLower().Contains(searchTerm.ToLower())),
                    true,
                    a => a.Category,
                    t => t.Tags
                );
            }
            else if (User.IsInRole("Staff"))
            {
                articles = _articleService.GetBy(
                    x => x.NewsTitle.ToLower().Contains(searchTerm.ToLower()),
                    true,
                    a => a.Category,
                    t => t.Tags
                );
            }


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

        /// <summary>
        /// Add Article
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddArticle(AddNewArticleRequest model)
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
                CreatedDate = DateTime.UtcNow,
                Tags = model.SelectedTags.Select(tagId => _tagService.GetById(tagId)).ToList(),
                CreatedById = GetCurrentUserId(),
            };

            _articleService.AddNewsArticle(newArticle);

            // ✅ Gửi dữ liệu bài viết mới qua SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", new
            {
                actionType = "Create",
                newsId = newArticle.NewsArticleId,
                newsTitle = newArticle.NewsTitle,
                headline = newArticle.Headline,
                createdAt = newArticle.CreatedDate,
                newsContent = newArticle.NewsContent,
                newsSource = newArticle.NewsSource,
                categoryId = newArticle.CategoryId,
                newsStatus = newArticle.NewsStatus,
                createdById = newArticle.CreatedById,
                tags = newArticle.Tags?.Select(t => t.TagName).ToList(),
                categoryName = _categoryService.GetById((short)model.SelectedCategory).CategoryName,
            });

            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult GetLatestArticles()
        {
            var articles = _articleService.GetBy().OrderByDescending(a => a.NewsArticleId).Take(10);
            return PartialView("_NewsListPartial", articles);
        }
        /// <summary>
        /// Update Article
        /// </summary>
        /// <param name="updatedArticle"></param>
        /// <param name="SelectedTags"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateArticle(EditNewsArticleVM updatedArticle, List<int> SelectedTags)
        {
            var existingArticle = _articleService.GetById(updatedArticle.NewsArticleId);

            if (existingArticle != null)
            {
                existingArticle.NewsTitle = updatedArticle.NewsTitle;
                existingArticle.NewsContent = updatedArticle.NewsContent;
                existingArticle.CategoryId = (short?)updatedArticle.SelectedCategory;
                existingArticle.NewsStatus = updatedArticle.NewsStatus;
                foreach (var tagId in SelectedTags)
                {
                    var tag = _tagService.GetById(tagId);
                    if (tag != null)
                    {
                        existingArticle.Tags.Add(tag);
                    }
                }
                TempData["ToastMessage"] = "Article Update successfully!";
                TempData["ToastType"] = "success";
                _articleService.UpdateNewsArticle(existingArticle);

                await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", new
                {
                    actionType = "Update",
                    newsId = updatedArticle.NewsArticleId,
                    newsTitle = updatedArticle.NewsTitle,
                    headline = updatedArticle.HeadLine,
                    newsContent = updatedArticle.NewsContent,
                    categoryId = updatedArticle.SelectedCategory,
                    newsStatus = updatedArticle.NewsStatus,
                    tags = existingArticle.Tags?.Select(t => t.TagName).ToList() ?? new List<string>(),
                    categoryName = _categoryService.GetById((short)updatedArticle.SelectedCategory).CategoryName,
                });

                return RedirectToAction("Index");
            }
            return NotFound();
        }
        /// <summary>
        /// Delete Article
        /// </summary>
        /// <param name="newsArticleId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteArticle(string newsArticleId)
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

            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", new
            {
                actionType = "Delete",
                newsId = newsArticleId
            });

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Get Current User Id
        /// </summary>
        /// <returns></returns>
        private short GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? short.Parse(userIdClaim.Value) : (short)0;
        }
        /// <summary>
        /// Get Next Id Of Article
        /// </summary>
        /// <returns></returns>
        private string GetNextNewsArticleId()
        {
            var articles = _articleService.GetBy();

            int maxId = articles
                .ToList()
                .Max(a => int.Parse(a.NewsArticleId));

            return (maxId + 1).ToString();
        }

        /// <summary>
        /// Get Search Term
        /// </summary>
        /// <returns></returns>
        public string GetSearchTerm()
        {
            var searchTerm = HttpContext.Request.Query["searchTerm"].ToString();
            return searchTerm;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}