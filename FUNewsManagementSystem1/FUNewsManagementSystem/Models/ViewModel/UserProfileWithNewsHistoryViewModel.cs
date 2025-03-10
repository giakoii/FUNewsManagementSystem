namespace FUNewsManagementSystem.Models.ViewModel;

public class UserProfileWithNewsHistoryViewModel
{
    public UserProfileViewModel UserProfile { get; set; }
    public List<NewsArticleHistoryViewModel> NewsArticleHistory { get; set; }
}