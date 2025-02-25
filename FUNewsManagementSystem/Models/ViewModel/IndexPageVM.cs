using DataAccessObject.Models;

namespace FUNewsManagementSystem.Models.ViewModel
{
    
    public class IndexPageVM
    {
        public IEnumerable<NewsArticle> Articles { get; set; }
        public CreateNewsArticleVM CreateVM { get; set; }
    }
}
