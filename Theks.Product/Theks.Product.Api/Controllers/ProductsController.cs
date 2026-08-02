using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Theks.Product.Application.DTOs;
using Theks.Product.Application.Interfaces;
using Theks.Shared.Responses;

namespace Theks.Product.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Application.DTOs.Product>>> GetProducts(CancellationToken cancellationToken)
    {
        var products = await productService.GetAllAsync(cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Application.DTOs.Product>> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var product = await productService.FindByIdAsync(id, cancellationToken);
        
        if (product is null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Product Not Found",
                Detail = $"The product with ID '{id}' was not found.",
                Instance = HttpContext.Request.Path
            });
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateProduct(Application.DTOs.Product productDto, CancellationToken cancellationToken)
    {
        // Note: [ApiController] handles validation automatically, but manual checks are kept for safety
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await productService.CreateAsync(productDto, cancellationToken);

        return result.Flag ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<ActionResult<Response>> UpdateProduct(Application.DTOs.Product productDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await productService.UpdateAsync(productDto, cancellationToken);

        return result.Flag ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Response>> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        var result = await productService.DeleteAsync(id, cancellationToken);

        return result.Flag ? Ok(result) : BadRequest(result);
    }
}
