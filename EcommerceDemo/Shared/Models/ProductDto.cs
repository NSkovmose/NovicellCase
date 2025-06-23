namespace Shared.Models;
public class ProductDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Image { get; set; }
}

