using DataAccessObject.Models;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Hubs;

public class NewsHub : Hub
{
    public async Task SendNewsUpdate(NewsArticle article, string actionType)
    {
        await Clients.All.SendAsync("ReceiveNewsUpdate", new
        {
            actionType,
            newsId = article.NewsArticleId,
            newsTitle = article.NewsTitle,
            headline = article.Headline,
            createdAt = article.CreatedDate,
            newsContent = article.NewsContent,
            newsSource = article.NewsSource,
            categoryId = article.CategoryId,
            newsStatus = article.NewsStatus,
            createdById = article.CreatedById,
            tags = article.Tags?.Select(t => t.TagName).ToList(),
            categoryName = article.Category?.CategoryName
        });
    }
}