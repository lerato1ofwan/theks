using Microsoft.AspNetCore.Mvc;
using Theks.Product.Application.DTOs;
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
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await productRepository.GetAllAsync();

        var (_, list) = ProductMapper.FromEntity(null!, products);

        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
    {
        var productResult = await productRepository.FindByIdAsync(id);

        var (product, _) = ProductMapper.FromEntity(productResult, null);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Response>> CreateProduct(ProductDto productDto)
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
    public async Task<ActionResult<Response>> UpdateProduct(ProductDto productDto)
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
    public async Task<ActionResult<Response>> DeleteProduct(ProductDto productDto)
    {
        var requestEntity = ProductMapper.ToEntity(productDto);

        var result = await productRepository.DeleteAsync(requestEntity);

        return result.Flag ? Ok(result) : BadRequest(result);
    }
}