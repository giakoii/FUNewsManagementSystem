using BusinessLogic.Service;
using BusinessObject.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem2.Pages.Admin;

public class Dashboard : PageModel
{
    private readonly ISystemAccountService _systemAccountService;
    private readonly INewArticleService _newsService;

    public Dashboard(ISystemAccountService systemAccountService, INewArticleService newsService)
    {
        _systemAccountService = systemAccountService;
        _newsService = newsService;
    }

    public void OnGet()
    {
        
    }
}