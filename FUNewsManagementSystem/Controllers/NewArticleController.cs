using BusinessObject.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    public class NewArticleController : Controller
    {
        private readonly INewArticleService _articleService;

        public NewArticleController(INewArticleService articleService)
        {
            _articleService = articleService;
        }
        /// <summary>
        /// Get New Article
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            if (User.IsInRole("Staff"))
            {
                return View(_articleService.GetAllNewsArticles());
            }
            var articles = _articleService.GetAllNewsArticles().Where(x => x.NewsStatus == true);
            return View(articles);
        }
    }
}
