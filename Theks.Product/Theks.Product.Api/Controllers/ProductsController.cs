using Microsoft.AspNetCore.Mvc;
using Theks.Product.Application.DTOs.Mappings;
using Theks.Product.Application.Interfaces;
using Theks.Shared.Responses;

namespace Theks.Product.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController
    (IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Application.DTOs.Product>>> GetProducts()
    {
        var products = await productRepository.GetAllAsync();

        var (_, list) = ProductMapper.FromEntity(null!, products);

        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Application.DTOs.Product>> GetProduct(Guid id)
    {
        var productResult = await productRepository.FindByIdAsync(id);
        if (productResult is null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Product Not Found",
                Detail = $"The product with ID '{id}' was not found",
                Instance = HttpContext.Request.Path
            });
        }

        var (product, _) = ProductMapper.FromEntity(productResult, null);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateProduct(Application.DTOs.Product productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestEntity = ProductMapper.ToEntity(productDto);

        var result = await productRepository.CreateAsync(requestEntity);

        return result.Flag ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<Response>> UpdateProduct(Application.DTOs.Product productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestEntity = ProductMapper.ToEntity(productDto);

        var result = await productRepository.UpdateAsync(requestEntity);

        return result.Flag ? Ok(result) : BadRequest(result);
    }

    [HttpDelete]
    public async Task<ActionResult<Response>> DeleteProduct(Application.DTOs.Product productDto)
    {
        var requestEntity = ProductMapper.ToEntity(productDto);

        var result = await productRepository.DeleteAsync(requestEntity);

        return result.Flag ? Ok(result) : BadRequest(result);
    }
}