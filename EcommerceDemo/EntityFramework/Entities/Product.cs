namespace EntityFramework.Entities;

public class Product
{
    public string? ETag { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public string Image { get; set; }

    public Category Category { get; set; }
}