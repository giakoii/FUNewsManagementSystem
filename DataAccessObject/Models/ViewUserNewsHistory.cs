namespace DataAccessObject.Models;

public partial class ViewUserNewsHistory
{
    public string NewsArticleId { get; set; } = null!;

    public string? NewsTitle { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? NewsContent { get; set; }

    public string? NewsSource { get; set; }

    public short? CategoryId { get; set; }

    public short? CreatedById { get; set; }

    public string? CreatedByName { get; set; }
}
