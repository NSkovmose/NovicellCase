namespace EntityFramework.Entities;

public class Category
{
    public string? ETag { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Product> Products { get; set; }
}