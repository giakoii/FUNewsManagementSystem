namespace FUNewsManagementSystem.Models.ViewModel
{
    public class AddNewArticleRequest
    {
        public string NewsTitle { get; set; }
        public string HeadLine { get; set; }
        public string NewSource { get; set; }
        public string NewsContent { get; set; }
        public int SelectedCategory { get; set; }
        public List<int> SelectedTags { get; set; }
    }
}
