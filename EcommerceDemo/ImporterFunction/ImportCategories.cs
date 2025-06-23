using EntityFramework;
using EntityFramework.Entities;
using ImporterFunction.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class ImportCategoriesFunction(DatabaseContext db, ILogger<ImportCategoriesFunction> logger)
{
    [Function("ImportCategories")]
    public async Task Run([TimerTrigger("0 0 */6 * * *", RunOnStartup = true)] TimerInfo timer)
    {
        logger.LogInformation("Category import started at: {time}", DateTime.Now);

        var json = await File.ReadAllTextAsync("data/categories.json");
        var imports = JsonSerializer.Deserialize<List<ImportCategory>>(json);
        //var dbRedis = redis.GetDatabase();

        if (imports != null)
        {
            foreach (var cat in imports)
            {
                var exists = await db.Categories.FindAsync(cat.Id);
                var categoryChanged = false;

                if (exists == null)
                {
                    db.Categories.Add(new Category
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                        Description = cat.Description,
                        ETag = cat.ETag
                    });
                    categoryChanged = true;
                }
                else if (exists.ETag != cat.ETag)
                {
                    exists.Name = cat.Name;
                    exists.Description = cat.Description;
                    exists.ETag = cat.ETag;
                    categoryChanged = true;
                }
                else
                {
                    logger.LogInformation("Skipping category {Id} — no changes detected", cat.Id);
                    continue;
                }

                // REDIS MOCK

                //if (categoryChanged)
                //{
                //    var productIds = await dbRedis.SetMembersAsync($"category:{cat.Id}:products");

                //    foreach (var productId in productIds)
                //    {
                //        var productKey = $"product:{productId}";
                //        var cached = await dbRedis.StringGetAsync(productKey);
                //        if (!cached.HasValue) continue;

                //        var productDto = JsonSerializer.Deserialize<ProductDto>(cached!);
                //        if (productDto == null) continue;

                //        productDto.CategoryName = cat.Name;

                //        await dbRedis.StringSetAsync(
                //            productKey,
                //            JsonSerializer.Serialize(productDto),
                //            TimeSpan.FromMinutes(10));
                //    }

                //    var categoryDto = new CategoryDto
                //    {
                //        Id = cat.Id,
                //        Name = cat.Name,
                //        Description = cat.Description
                //    };
                //    await dbRedis.StringSetAsync($"category:{cat.Id}", JsonSerializer.Serialize(categoryDto));
                //}
            }

            await db.SaveChangesAsync();
        }

        logger.LogInformation("Category import completed at: {time}", DateTime.Now);
    }
}
