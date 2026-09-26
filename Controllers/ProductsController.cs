using Microsoft.AspNetCore.Mvc;
using EcommerceBackend.Services;
using EcommerceBackend.Entities;
using EcommerceBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
namespace EcommerceBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search ,
            [FromQuery] int? categoryId,
            [FromQuery] decimal? minPrice ,
            [FromQuery] decimal? maxPrice )
        {
            var products = await _productService.GetAllAsync(search, categoryId, minPrice, maxPrice);
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Product product)
        {
            var createdProduct = await _productService.CreatAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.ProductId }, createdProduct);
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            var  updatedProduct = await _productService.UpdateAsync(product);
            if (!updatedProduct )
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPost("{id:int}/stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdjustStock (int id , StockAdjustmentDto dto)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();
            if (product.ProductQuantity + dto.Quantity < 0)
            {
                return BadRequest("cannot be negative");
            }
            product.ProductQuantity += dto.Quantity;

            await _productService.UpdateAsync(product);
            return Ok(new { product.ProductId, product.ProductQuantity, dto.Reason });
        }
        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 5)
        {
            var products = await _productService.GetAllAsync(null, null, null, null);
            var lowStock = products.Where(p => p.ProductQuantity <= threshold);
            return Ok(lowStock);
        }
    }
}
