using API.Models.Requests;
using API.Models.Responses;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers;

[ApiController]
[Route("products")]
public class ProductsController(IProductService productService) : ControllerBase
{

    /// <summary>
    /// Returns a paginated list of products with optional search and category filter.
    /// </summary>
    /// <param name="query">Paging and filter parameters</param>
    /// <returns>A paginated result of products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductDto>), 200)]
    public async Task<IActionResult> SearchProducts([FromQuery] ProductSearchQueryParams query)
    {
        var (items, total) = await productService.SearchProductsAsync(query.Search ?? string.Empty, query.Page, query.PageSize);

        var result = new PagedResult<ProductDto>
        {
            Total = total,
            Page = query.Page,
            PageSize = query.PageSize,
            Items = items
        };

        return new JsonResult(result);
    }

    /// <summary>
    /// Returns a paginated list of products for a given category.
    /// </summary>
    /// <param name="query">Category ID with paging parameters</param>
    /// <returns>A paginated result of products</returns>
    [HttpGet("by-category")]
    [ProducesResponseType(typeof(PagedResult<ProductDto>), 200)]
    public async Task<IActionResult> GetProductsByCategory([FromQuery] CategoryQueryParams query)
    {
        if (string.IsNullOrWhiteSpace(query.Category))
        {
            return BadRequest("Please provide a category id");
        }

        var (items, total) = await productService.GetByCategoryAsync(query.Category, query.Page, query.PageSize);

        var result = new PagedResult<ProductDto>
        {
            Total = total,
            Page = query.Page,
            PageSize = query.PageSize,
            Items = items
        };

        return new JsonResult(result);
    }



    /// <summary>
    /// Returns a single product based on Id.
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>A single product</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), 200)]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await productService.GetByIdAsync(id);
        if (product is null)
            return NotFound();

        return new JsonResult(product);
    }


}
