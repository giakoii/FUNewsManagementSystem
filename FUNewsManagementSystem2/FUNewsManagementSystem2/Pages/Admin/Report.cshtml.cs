using AutoMapper;
using BusinessLogic.DTOs;
using BusinessObject.Service;
using FUNewsManagementSystem2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Admin;

public class Report : PageModel
{
    private readonly INewArticleService _newArticleService;
    private readonly IMapper _mapper;

    public Report(INewArticleService newArticleService, IMapper mapper)
    {
        _newArticleService = newArticleService;
        _mapper = mapper;
    }
    [BindProperty(SupportsGet = true)]
    public DateTime? StartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? EndDate { get; set; }

    public List<NewsArticleViewModel> NewsArticles { get; set; } = new();

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

        var newsArticleDtos = _newArticleService.GetNewsReportByDateRange(StartDate.Value, EndDate.Value) ?? new List<NewsArticleDto>();
        NewsArticles = _mapper.Map<List<NewsArticleViewModel>>(newsArticleDtos);
    }
}