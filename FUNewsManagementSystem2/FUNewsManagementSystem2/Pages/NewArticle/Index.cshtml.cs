using AutoMapper;
using BusinessLogic.Service;
using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Hubs;
using FUNewsManagementSystem.Models.ViewModel;
using FUNewsManagementSystem2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

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

            IEnumerable<NewsArticle> articlesQuery = Enumerable.Empty<NewsArticle>();

            if (!User.Identity.IsAuthenticated || User.IsInRole("Lecturer"))
            {
                // Chỉ lấy bài viết NewsStatus == true
                articlesQuery = _articleService.GetBy(
                    x => (x.NewsStatus == true),
                    true,
                    a => a.Category,
                    t => t.Tags
                );
            }
            else if (User.IsInRole("Staff"))
            {
                articlesQuery = _articleService.GetBy(
                    null,
                    true,
                    a => a.Category,
                    t => t.Tags
                );
            }

            var articlesList = articlesQuery.ToList();
            Articles = _mapper.Map<List<NewsArticleViewModel>>(articlesList);

            var tags = _tagService.GetBy(); // trả về IEnumerable<Tag>
            Tags = _mapper.Map<List<TagViewModel>>(tags);

            var categories = _categoryService.GetBy();
            Categories = _mapper.Map<List<CategoryViewModel>>(categories) ?? new List<CategoryViewModel>();
        }

        public void OnGet()
        {
            LoadData();
        }

        public async Task<IActionResult> OnPostAddArticle()
        {
            //if (!ModelState.IsValid)
            //{
            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine(error.ErrorMessage);
            //    }
            //    LoadData();
            //    TempData["ToastMessage"] = "Invalid data!";
            //    TempData["ToastType"] = "danger";
            //    return Page();
            //}

            if (NewArticle == null)
            {
                TempData["ToastMessage"] = "Article data is missing!";
                TempData["ToastType"] = "danger";
                return Page();
            }

            var newArticle = _mapper.Map<NewsArticle>(NewArticle);

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
                newsStatus = newArticle.NewsStatus,
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

            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", new
            {
                actionType = "Update",
                newsId = existingArticle.NewsArticleId,
                newsTitle = existingArticle.NewsTitle,
                headline = existingArticle.Headline,
                newsContent = existingArticle.NewsContent,
                categoryId = existingArticle.CategoryId,
                newsStatus = existingArticle.NewsStatus,
                tags = existingArticle.Tags?.Select(t => t.TagName).ToList() ?? new List<string>(),
                categoryName = existingArticle.Category?.CategoryName
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

    }

}