namespace FUNewsManagementSystem.Models.ViewModel
{
    public class EditNewsArticleVM
    {
        public string NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public string HeadLine { get; set; }
        public string NewSource { get; set; }
        public string NewsContent { get; set; }
        public bool NewsStatus { get; set; }
        public int SelectedCategory { get; set; }
        public List<int> SelectedTags { get; set; }
    }
}
