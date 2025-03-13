namespace BusinessLogic.DTOs;

public class VwUserNewsHistoryDto
{
    public string NewsArticleId { get; set; }

    public string? NewsTitle { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? NewsContent { get; set; }

    public string? NewsSource { get; set; }

    public short? CategoryId { get; set; }

    public short? CreatedById { get; set; }

    public string? CreatedByName { get; set; }
}