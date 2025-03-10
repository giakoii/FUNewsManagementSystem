using AutoMapper;
using BusinessLogic.DTOs;
using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem2.Pages.NewArticle;

public class Index : PageModel
{
    private readonly INewArticleService _articleService;
    private readonly ITagService _tagService;
    private readonly ICategoryService _categoryService;
    private readonly IHubContext<NewsHub> _hubContext;
    
    private readonly IMapper _mapper;

    public List<NewsArticleDto> Articles { get; set; }
    public List<Category> Categories { get; set; }
    public List<Tag> Tags { get; set; }
    [BindProperty(SupportsGet = true)] public string SearchTerm { get; set; }
    [BindProperty(SupportsGet = true)] public string SortBy { get; set; } = "Title";
    [BindProperty(SupportsGet = true)] public string SortOrder { get; set; } = "asc";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="articleService"></param>
    /// <param name="tagService"></param>
    /// <param name="categoryService"></param>
    /// <param name="hubContext"></param>
    public Index(INewArticleService articleService, ITagService tagService, ICategoryService categoryService, IHubContext<NewsHub> hubContext)
    {
        _articleService = articleService;
        _tagService = tagService;
        _categoryService = categoryService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Get New Article
    /// </summary>
    public void OnGet()
    {
        // Get all categories, tags, and articles
        // Categories = _categoryService.GetBy().ToList();
        // Tags = _tagService.GetBy().ToList();
        // Articles = _articleService.GetBy(
        //     x => x.NewsStatus == true && x.NewsTitle.ToLower().Contains(SearchTerm.ToLower() ?? ""),
        //     true, a => a.Category, t => t.Tags
        // ).ToList();
        //
        // // Sort articles
        // Articles = SortBy switch
        // {
        //     "Title" => SortOrder == "asc" ? Articles.OrderBy(a => a.NewsTitle).ToList() : Articles.OrderByDescending(a => a.NewsTitle).ToList(),
        //     "Id" => SortOrder == "asc" ? Articles.OrderBy(a => a.NewsArticleId).ToList() : Articles.OrderByDescending(a => a.NewsArticleId).ToList(),
        //     "Date" => SortOrder == "asc" ? Articles.OrderBy(a => a.CreatedDate).ToList() : Articles.OrderByDescending(a => a.CreatedDate).ToList(),
        //     _ => Articles
        // };
    }
}