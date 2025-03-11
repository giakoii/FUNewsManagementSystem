using AutoMapper;
using BusinessLogic.DTOs;
using BusinessObject.Service;
using DataAccessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.NewArticle;

public class Add : PageModel
{
    private readonly IMapper _mapper;
    private readonly INewArticleService _articleService;
    private readonly ITagService _tagService;
    private readonly ICategoryService _categoryService;

    public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    public List<TagDto> Tags { get; set; } = new List<TagDto>();

    [BindProperty] public NewsArticleDto NewArticle { get; set; } 
    [BindProperty] public List<int> SelectedTags { get; set; }

    public Add(IMapper mapper, INewArticleService articleService, ITagService tagService, ICategoryService categoryService)
    {
        _mapper = mapper;
        _articleService = articleService;
        _tagService = tagService;
        _categoryService = categoryService;
    }
    
    public IActionResult OnGet()
    {
        ModelState.Clear(); // 🔥 Reset ModelState để lấy dữ liệu mới nhất

        var categoryEntities = _categoryService.GetBy()?.ToList();
        var tagEntities = _tagService.GetBy()?.ToList();

        Console.WriteLine($"Categories Count: {categoryEntities?.Count ?? 0}");
        Console.WriteLine($"Tags Count: {tagEntities?.Count ?? 0}");

        if (categoryEntities != null && categoryEntities.Any())
        {
            Categories = _mapper.Map<List<CategoryDto>>(categoryEntities);
        }

        if (tagEntities != null && tagEntities.Any())
        {
            Tags = _mapper.Map<List<TagDto>>(tagEntities);
        }

        Console.WriteLine($"Mapped Categories Count: {Categories.Count}");
        Console.WriteLine($"Mapped Tags Count: {Tags.Count}");

        return Page(); // 🔥 Đảm bảo trả về dữ liệu cho Razor Page
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    Console.WriteLine($"Validation Error - {modelStateKey}: {error.ErrorMessage}");
                }
            }
            return Page();
        }

        var article = _mapper.Map<NewsArticle>(NewArticle);
        article.Tags = _tagService.GetBy(t => SelectedTags.Contains(t.TagId)).ToList();

        _articleService.AddNewsArticle(article);
        return RedirectToPage("NewArticle/Index"); // 🔥 Đảm bảo chuyển hướng đúng trang sau khi lưu
    }

}