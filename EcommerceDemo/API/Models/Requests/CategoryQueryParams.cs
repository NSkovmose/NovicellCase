namespace API.Models.Requests;

public class CategoryQueryParams
{
    public string? Category { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
