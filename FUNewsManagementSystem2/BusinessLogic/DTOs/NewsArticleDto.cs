using DataAccessObject.Models;

namespace BusinessLogic.DTOs;

public class NewsArticleDto
{
    public string NewsArticleId { get; set; }
    public string NewsTitle { get; set; }
    public string Headline { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string NewsContent { get; set; }
    public string NewsSource { get; set; }
    
    public short? CategoryId { get; set; }
    public string CategoryName { get; set; }
    
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public List<int> TagIds { get; set; } = new();
    public List<string> TagNames { get; set; } = new();

    public bool? NewsStatus { get; set; }

    public short? CreatedById { get; set; }
    public short? UpdatedById { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
