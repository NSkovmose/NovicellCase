using API.Services.Interfaces;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Services;

public class ProductService(DatabaseContext db) : IProductService
{
    public async Task<(IEnumerable<ProductDto> Items, int TotalCount)> SearchProductsAsync(string search, int page, int pageSize)
    {
        var query = db.Products
            .AsNoTracking()
            .Where(p =>
                EF.Functions.Like(p.Title, $"%{search}%") ||
                EF.Functions.Like(p.Description, $"%{search}%"))
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                Image = p.Image,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            });

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetByCategoryAsync(string categoryId, int page, int pageSize)
    {
        // Simulated Redis logic

        // var cacheKey = $"category:{categoryId}:products";
        // var productIds = await redis.SetMembersAsync(cacheKey);

        // if (productIds.Length > 0)
        // {
        //     var products = new List<ProductDto>();
        //     foreach (var id in productIds)
        //     {
        //         var cachedProduct = await redis.StringGetAsync($"product:{id}");
        //         if (cachedProduct.HasValue)
        //         {
        //             var dto = JsonSerializer.Deserialize<ProductDto>(cachedProduct!);
        //             if (dto != null)
        //                 products.Add(dto);
        //         }
        //     }

        //     var total = products.Count;
        //     var items = products
        //         .Skip((page - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToList();

        //     return (items, total);
        // }

        var query = db.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == categoryId)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                Image = p.Image,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            });

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }


    public async Task<ProductDto?> GetByIdAsync(string id)
    {

        // Simulate Redis cache access (pseudo-code)

        //var cacheKey = $"product:{id}";

        // Try to get from Redis first
        //var cached = await redis.GetStringAsync(cacheKey);
        //if (cached.HasValue)
        //{
        //    var dto = JsonSerializer.Deserialize<ProductDto>(cached!);
        //    if (dto != null)
        //        return dto;
        //}

        var result = await db.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                Image = p.Image,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            })
            .FirstOrDefaultAsync();


        // Cache result in Redis
        //await redis.StringSetAsync(
        //    cacheKey,
        //    JsonSerializer.Serialize(result),
        //    TimeSpan.FromMinutes(10));

        return result;
    }
}
