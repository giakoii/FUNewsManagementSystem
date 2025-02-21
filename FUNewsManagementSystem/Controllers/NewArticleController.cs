using BusinessObject.Service;
using DataAccessObject.Models;
using FUNewsManagementSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    public class NewArticleController : Controller
    {
        private readonly INewArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly FUNewsManagementSystemContext _context;
        public NewArticleController(
            INewArticleService articleService,
            ICategoryService categoryService,
            ITagService tagService, FUNewsManagementSystemContext context)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = new IndexPageVM
            {
                Articles = _articleService.GetAllNewsArticles(),
                CreateVM = new CreateNewsArticleVM
                {
                    Categories = _categoryService.GetAllCategories(),
                    Tags = _tagService.GetAllTags()
                }
            };
            return View(vm);
        }
    
    [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateNewsArticleVM
            {
                Categories = _categoryService.GetAllCategories(),
                Tags = _tagService.GetAllTags()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateNewsArticleVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoryService.GetAllCategories();
                model.Tags = _tagService.GetAllTags();
                return View(model);
            }
            var currentUserId = short.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUser = _context.SystemAccounts.Find(currentUserId);
            var newsArticle = new NewsArticle
            {
                NewsArticleId = GenerateArticleId(),
                NewsTitle = model.NewsTitle,
                Headline = model.Headline, // Đã sửa chính tả
                NewsContent = model.NewsContent,
                NewsSource = model.NewsSource,
                CategoryId = model.CategoryId.Value,
                CreatedById = short.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), // Thêm claim type
                Tags = _tagService.GetTagsByIds(model.SelectedTagIds).ToList(), // Đúng tên property
                NewsStatus = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = currentUser,
            };

            var result = _articleService.AddNewsArticle(newsArticle);

            if (result)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error creating article");
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var article = _articleService.GetNewsArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            var editVM = new EditNewsArticleVM
            {
                NewsArticleId = article.NewsArticleId,
                NewsTitle = article.NewsTitle,
                Headline = article.Headline,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                CategoryId = article.CategoryId,
                SelectedTagIds = article.Tags.Select(t => t.TagId).ToList(),
                Categories = _categoryService.GetAllCategories(),
                Tags = _tagService.GetAllTags()
            };

            return View(editVM);
        }
        [HttpPost]
        public IActionResult Edit(EditNewsArticleVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoryService.GetAllCategories();
                model.Tags = _tagService.GetAllTags();
                return View(model);
            }

            var existingArticle = _articleService.GetNewsArticleById(model.NewsArticleId);
            if (existingArticle == null)
            {
                return NotFound();
            }

            existingArticle.NewsTitle = model.NewsTitle;
            existingArticle.Headline = model.Headline;
            existingArticle.NewsContent = model.NewsContent;
            existingArticle.NewsSource = model.NewsSource;
            existingArticle.CategoryId = model.CategoryId.Value;
            existingArticle.ModifiedDate = DateTime.Now;

            // Cập nhật tags
            existingArticle.Tags.Clear();
            var selectedTags = _tagService.GetTagsByIds(model.SelectedTagIds).ToList();
            foreach (var tag in selectedTags)
            {
                existingArticle.Tags.Add(tag);
            }

            _articleService.UpdateNewsArticle(existingArticle);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var result = _articleService.DeleteNewsArticle(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Xóa bài viết thành công";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa bài viết";
            }
            return RedirectToAction("Index");
        }
        private string GenerateArticleId()
        {
            return "ART-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
