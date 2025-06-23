using EntityFramework;
using EntityFramework.Entities;
using ImporterFunction.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ImporterFunction;

public class ImportProductsFunction(DatabaseContext db, ILogger<ImportProductsFunction> logger)
{
    [Function("ImportProducts")]
    public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer)
    {
        logger.LogInformation("Product import started at: {time}", DateTime.Now);

        var json = await File.ReadAllTextAsync("data/products.json");
        var imports = JsonSerializer.Deserialize<List<ImportProduct>>(json);
        var categoryMap = await db.Categories
            .ToDictionaryAsync(c => c.Id, c => c.Name);

        if (imports != null)
        {
            foreach (var p in imports)
            {
                if (!categoryMap.ContainsKey(p.Category))
                {
                    logger.LogWarning("Skipping product {Id} — invalid category {CategoryId}", p.Id, p.Category);
                    continue;
                }

                var exists = await db.Products.FindAsync(p.Id);
                if (exists == null)
                {
                    db.Products.Add(new Product
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Price = p.Price,
                        Description = p.Description,
                        Image = p.Image,
                        CategoryId = p.Category,
                        ETag = p.ETag
                    });
                }
                else if (exists.ETag != p.ETag)
                {
                    //if (exists != null && exists.CategoryId != p.Category)
                    //{
                    //    await redis.SetRemoveAsync($"category:{exists.CategoryId}:products", p.Id);
                    //}

                    exists.Title = p.Title;
                    exists.Price = p.Price;
                    exists.Description = p.Description;
                    exists.Image = p.Image;
                    exists.CategoryId = p.Category;
                    exists.ETag = p.ETag;
                }
                else
                {
                    logger.LogInformation("Skipping product {Id} — no changes detected", p.Id);
                    continue;
                }

                // Update the Redis cache

                //var productDto = new ProductDto
                //{
                //    Id = p.Id,
                //    Title = p.Title,
                //    Price = p.Price,
                //    Description = p.Description,
                //    Image = p.Image,
                //    CategoryId = p.Category,
                //    CategoryName = categoryMap[p.Category]
                //};

                //await redis.StringSetAsync(
                //    $"product:{p.Id}",
                //    JsonSerializer.Serialize(productDto),
                //    TimeSpan.FromMinutes(10));

                // Maintain category index
                //await redis.SetAddAsync($"category:{p.Category}:products", p.Id);
            }
        }

        await db.SaveChangesAsync();
        logger.LogInformation("Product import completed at: {time}", DateTime.Now);
    }
}