using API.Models.Requests;
using EntityFramework.Entities;
using Shared.Models;

namespace API.Services.Interfaces;

public interface IProductService
{
    Task<(IEnumerable<ProductDto> Items, int TotalCount)> GetByCategoryAsync(string categoryId, int page, int pageSize);

    Task<(IEnumerable<ProductDto> Items, int TotalCount)> SearchProductsAsync(string search, int page, int pageSize);

    Task<ProductDto?> GetByIdAsync(string id);


}
