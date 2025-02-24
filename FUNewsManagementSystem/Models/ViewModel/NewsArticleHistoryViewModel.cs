using System.Collections;

namespace FUNewsManagementSystem.Models.ViewModel;

public class NewsArticleHistoryViewModel 
{
    public string NewsArticleId { get; set; } = null!;

    public string? NewsTitle { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? NewsContent { get; set; }

    public string? NewsSource { get; set; }

    public short? CategoryId { get; set; }
    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}