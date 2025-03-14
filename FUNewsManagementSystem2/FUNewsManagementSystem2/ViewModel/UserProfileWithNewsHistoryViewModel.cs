using FUNewsManagementSystem.Models.ViewModel;

namespace FUNewsManagementSystem2.ViewModel;

public class UserProfileWithNewsHistoryViewModel
{
    public UserProfileViewModel UserProfile { get; set; }

    public List<NewsArticleHistoryViewModel>? NewsArticleHistory { get; set; }
}