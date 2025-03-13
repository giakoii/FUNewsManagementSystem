using AutoMapper;
using BusinessLogic.Service;
using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Models.ViewModel;
using FUNewsManagementSystem2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.NewArticle
{
    public class Index : PageModel
    {
        private readonly IMapper _mapper;
        private readonly INewArticleService _articleService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;

        public List<NewsArticleViewModel> Articles { get; set; }
        public List<TagViewModel> Tags { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        // [BindProperty(SupportsGet = true)] public string SearchTerm { get; set; }
        // [BindProperty(SupportsGet = true)] public string SortBy { get; set; } = "Title";
        // [BindProperty(SupportsGet = true)] public string SortOrder { get; set; } = "asc";

        [BindProperty] public NewsArticleViewModel NewArticle { get; set; }
        [BindProperty] public List<int> SelectedTags { get; set; }

        public Index(IMapper mapper, INewArticleService articleService, ITagService tagService,
            ICategoryService categoryService)
        {
            _mapper = mapper;
            _articleService = articleService;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        public void OnGet()
        {
            var tags = _tagService.GetTags();
            Tags = _mapper.Map<List<TagViewModel>>(tags);

            var articles = _articleService.GetNewsArticles();
            Articles = _mapper.Map<List<NewsArticleViewModel>>(articles);

            var categories = _categoryService.GetBy();
            Categories = _mapper.Map<List<CategoryViewModel>>(categories);
        }

        public IActionResult OnPostAddArticle()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                TempData["ToastMessage"] = "Invalid data!";
                TempData["ToastType"] = "danger";
                return Page();
            }


            // Kiểm tra dữ liệu nhập vào
            if (NewArticle == null)
            {
                TempData["ToastMessage"] = "Article data is missing!";
                TempData["ToastType"] = "danger";
                return Page();
            }

            // Ánh xạ từ ViewModel sang Entity
            var newArticle = _mapper.Map<NewsArticle>(NewArticle);

            // Kiểm tra nếu có tag được chọn thì gán vào Article
            if (SelectedTags != null && SelectedTags.Any())
            {
                newArticle.Tags = _tagService.GetBy(t => SelectedTags.Contains(t.TagId)).ToList();
            }

            // Thêm bài viết mới
            _articleService.AddNewsArticle(newArticle);

            // Hiển thị thông báo thành công
            TempData["ToastMessage"] = "Article added successfully!";
            TempData["ToastType"] = "success";

            return RedirectToPage("./Index");
        }

    }
}