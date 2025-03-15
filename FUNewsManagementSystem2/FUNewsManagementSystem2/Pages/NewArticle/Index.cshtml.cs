using AutoMapper;
using BusinessLogic.Service;
using BusinessObject.Service;
using FUNewsManagementSystem.Hubs;
using FUNewsManagementSystem2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using BusinessLogic.DTOs;

namespace FUNewsManagementSystem2.Pages.NewArticle
{
    public class Index : PageModel
    {
        private readonly IMapper _mapper;
        private readonly INewArticleService _articleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly IHubContext<NewsHub> _hubContext;
        public List<NewsArticleViewModel> Articles { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        [BindProperty] public NewsArticleViewModel NewArticle { get; set; }
        [BindProperty] public List<int> SelectedTags { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortOption { get; set; }


        public Index(IMapper mapper, INewArticleService articleService, ITagService tagService,
            ICategoryService categoryService, IHubContext<NewsHub> hubContext)
        {
            _mapper = mapper;
            _articleService = articleService;
            _tagService = tagService;
            _categoryService = categoryService;
            _hubContext = hubContext;
        }

        public void LoadData()
        {
            IEnumerable<NewsArticleViewModel> articleViewModel = null;

            if (!User.Identity.IsAuthenticated || User.IsInRole("Lecturer"))
            {
                // Chỉ lấy bài viết NewsStatus == true
                var newsArticles = _articleService.GetBy(
                    x => (x.NewsStatus == true),
                    true,
                    a => a.Category,
                    t => t.Tags
                );
                articleViewModel = _mapper.Map<IEnumerable<NewsArticleViewModel> >(newsArticles);
            }
            else if (User.IsInRole("Staff"))
            {
                var articlesQuery = _articleService.GetBy(
                    null,
                    true,
                    a => a.Category,
                    t => t.Tags
                );
                articleViewModel = _mapper.Map<IEnumerable<NewsArticleViewModel> >(articlesQuery);

            }

            articleViewModel = SearchAndSort(articleViewModel);

            int totalCount = articleViewModel.Count();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            var pagedArticles = articleViewModel
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();


            var articlesList = articleViewModel.ToList();
            Articles = _mapper.Map<List<NewsArticleViewModel>>(pagedArticles);

            var tags = _tagService.GetBy(); // trả về IEnumerable<Tag>
            Tags = _mapper.Map<List<TagViewModel>>(tags);

            var categories = _categoryService.GetBy();
            Categories = _mapper.Map<List<CategoryViewModel>>(categories) ?? new List<CategoryViewModel>();
        }

        private IEnumerable<NewsArticleViewModel> SearchAndSort(IEnumerable<NewsArticleViewModel> articlesQuery)
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                articlesQuery = articlesQuery.Where(a => a.NewsTitle.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            switch (SortOption)
            {
                case "A-Z":
                    articlesQuery = articlesQuery.OrderBy(a => a.NewsTitle);
                    break;
                case "Z-A":
                    articlesQuery = articlesQuery.OrderByDescending(a => a.NewsTitle);
                    break;
                case "Oldest":
                    articlesQuery = articlesQuery.OrderBy(a => a.CreatedDate);
                    break;
                case "Newest":
                    articlesQuery = articlesQuery.OrderByDescending(a => a.CreatedDate);
                    break;
                default:
                    articlesQuery = articlesQuery.OrderByDescending(a => a.CreatedDate);
                    break;
            }

            return articlesQuery;
        }

        public void OnGet()
        {
            LoadData();
        }

        public async Task<IActionResult> OnPostAddArticle()
        {

            if (NewArticle == null)
            {
                TempData["ToastMessage"] = "Article data is missing!";
                TempData["ToastType"] = "danger";
                return Page();
            }

            var newArticle = _mapper.Map<NewsArticleDto>(NewArticle);

            newArticle.NewsArticleId = GetNextNewsArticleId();
            newArticle.CreatedDate = DateTime.UtcNow;
            newArticle.CreatedById = GetCurrentUserId();

            if (SelectedTags != null && SelectedTags.Any())
            {
                var existingTags = SelectedTags.Select(tagId => _tagService.GetById(tagId)).ToList();
                newArticle.Tags = existingTags;
            }

            _articleService.AddNewsArticle(newArticle);

            var catName = _categoryService.GetById((short)newArticle.CategoryId)?.CategoryName;

            TempData["ToastMessage"] = "Article added successfully!";
            TempData["ToastType"] = "success";

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
                newsStatus = true,
                createdById = newArticle.CreatedById,
                tags = newArticle.Tags?.Select(t => t.TagName).ToList(),
                categoryName = catName,
            });

            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostEditArticle()
        {
            var existingArticle = _articleService.GetById(NewArticle.NewsArticleId);
            if (existingArticle == null)
            {
                TempData["ToastMessage"] = "Article not found!";
                TempData["ToastType"] = "danger";
                return RedirectToPage("./Index");
            }
            existingArticle.NewsTitle = NewArticle.NewsTitle;
            existingArticle.NewsContent = NewArticle.NewsContent;
            existingArticle.CategoryId = NewArticle.CategoryId;
            existingArticle.NewsStatus = NewArticle.NewsStatus;
            if (SelectedTags != null && SelectedTags.Any())
            {
                foreach (var tagId in SelectedTags)
                {
                    var tag = _tagService.GetById(tagId);
                    if (tag != null)
                    {
                        existingArticle.Tags.Add(tag);
                    }
                }
            }
            _articleService.UpdateNewsArticle(existingArticle);

            var catName = _categoryService.GetById((short)existingArticle.CategoryId)?.CategoryName;

            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", new
            {
                actionType = "Update",
                newsId = existingArticle.NewsArticleId,
                newsTitle = existingArticle.NewsTitle,
                headline = existingArticle.Headline,
                createdAt = existingArticle.CreatedDate,
                newsContent = existingArticle.NewsContent,
                categoryId = existingArticle.CategoryId,
                newsStatus = existingArticle.NewsStatus,
                tags = existingArticle.Tags?.Select(t => t.TagName).ToList() ?? new List<string>(),
                categoryName = catName
            });

            TempData["ToastMessage"] = "Article updated successfully!";
            TempData["ToastType"] = "success";
            return RedirectToPage("./Index");
        }

        private string GetNextNewsArticleId()
        {
            var articles = _articleService.GetBy();

            int maxId = articles
                .ToList()
                .Max(a => int.Parse(a.NewsArticleId));

            return (maxId + 1).ToString();
        }

        private short GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? short.Parse(userIdClaim.Value) : (short)0;
        }

        /// <summary>
        /// Delete article
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteArticle()
        {
            if (string.IsNullOrEmpty(NewArticle.NewsArticleId))
            {
                TempData["ToastMessage"] = "Invalid article ID!";
                TempData["ToastType"] = "danger";
                return RedirectToPage();
            }

            var article = _articleService.GetById(NewArticle.NewsArticleId);
            if (article == null)
            {
                TempData["ToastMessage"] = "Article not found!";
                TempData["ToastType"] = "danger";
                return RedirectToPage();
            }

            _articleService.DeleteNewsArticle(article.NewsArticleId);

            TempData["ToastMessage"] = "Article deleted successfully!";
            TempData["ToastType"] = "success";

            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", new
            {
                actionType = "Delete",
                newsId = article.NewsArticleId
            });

            return RedirectToPage();
        }
    }
}