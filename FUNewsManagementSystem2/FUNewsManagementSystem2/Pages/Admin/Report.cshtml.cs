using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Service;
using BusinessObject.Service;
using FUNewsManagementSystem2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Admin;

public class Report : PageModel
{
    private readonly INewArticleService _newArticleService;
    private readonly ICategoryService _categoryService; // Thêm Category Service
    private readonly ISystemAccountService _accountService; // Thêm Account Service
    private readonly IMapper _mapper;

    public Report(INewArticleService newArticleService,
                 ICategoryService categoryService,
                 ISystemAccountService accountService,
                 IMapper mapper)
    {
        _newArticleService = newArticleService;
        _categoryService = categoryService;
        _accountService = accountService;
        _mapper = mapper;
    }

    [BindProperty(SupportsGet = true)]
    public DateTime? StartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? EndDate { get; set; }

    public List<NewsArticleViewModel> NewsArticles { get; set; } = new();
    public List<CategoryStatViewModel> CategoryStats { get; set; } = new(); 
    public int TotalUsers { get; set; } 

    public void OnGet()
    {
        DateTime minDate = new DateTime(1753, 1, 1);
        DateTime maxDate = DateTime.Now;

        if (StartDate > EndDate)
        {
            ModelState.AddModelError(string.Empty, "Start date cannot be greater than end date.");
            return;
        }

        if (EndDate.HasValue && EndDate.Value > DateTime.Today)
        {
            EndDate = DateTime.Today;
        }

        if (StartDate == null || StartDate < minDate)
            StartDate = DateTime.Today.AddDays(-30);
        if (EndDate == null || EndDate > maxDate)
            EndDate = maxDate;

  
        var newsArticleDtos = _newArticleService.GetNewsReportByDateRange(StartDate.Value, EndDate.Value)
            ?? new List<NewsArticleDto>();
        NewsArticles = _mapper.Map<List<NewsArticleViewModel>>(newsArticleDtos);

  
        var categories = _categoryService.GetBy();
        CategoryStats = categories.Select(c => new CategoryStatViewModel
        {
            CategoryName = c.CategoryName,
            ArticleCount = NewsArticles.Count(a => a.CategoryId == c.CategoryId)
        }).Where(c => c.ArticleCount > 0).ToList(); 


        TotalUsers = _accountService.GetAllUsers().Count; 
    }
}


public class CategoryStatViewModel
{
    public string CategoryName { get; set; }
    public int ArticleCount { get; set; }
}