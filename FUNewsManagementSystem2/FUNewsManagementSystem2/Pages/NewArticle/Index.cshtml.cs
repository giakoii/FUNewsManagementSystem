using AutoMapper;
using BusinessLogic.DTOs;
using BusinessObject.Service;
using DataAccessObject.Models;
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

        public List<NewsArticleDto> Articles { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<TagDto> Tags { get; set; }

        [BindProperty(SupportsGet = true)] public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)] public string SortBy { get; set; } = "Title";
        [BindProperty(SupportsGet = true)] public string SortOrder { get; set; } = "asc";

        [BindProperty] public NewsArticleDto NewArticle { get; set; }
        [BindProperty] public List<int> SelectedTags { get; set; }

        public Index(IMapper mapper, INewArticleService articleService, ITagService tagService, ICategoryService categoryService)
        {
            _mapper = mapper;
            _articleService = articleService;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        public void OnGet()
        {
            Categories = _mapper.Map<List<CategoryDto>>(_categoryService.GetBy().ToList());
            Tags = _mapper.Map<List<TagDto>>(_tagService.GetBy().ToList());

            var articles = _articleService.GetBy(x => x.NewsStatus == true
                , false
                , x => x.Category
                , x => x.Tags).ToList();
            Articles = _mapper.Map<List<NewsArticleDto>>(articles);
        }

        public IActionResult OnPostAddArticle()
        {
            if (!ModelState.IsValid) return Page();

            var article = _mapper.Map<NewsArticle>(NewArticle);
            article.Tags = _tagService.GetBy(t => SelectedTags.Contains(t.TagId)).ToList();

            _articleService.AddNewsArticle(article);
            return RedirectToPage();
        }
    }
}
