using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductManager _productManager;
        public ProductController(IProductManager productManager, IWebHostEnvironment webHostEnvironment)
        {
            _productManager = productManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productManager.GetAllProductsAsync();

            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("Pagination")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetAllProductsByPagination
            ([FromQuery] PaginationParameters paginationParameters,
            [FromQuery] ProductFilter productFilter)
        {
            var result = await _productManager.GetAllProductsByPaginationAsync(paginationParameters, productFilter);

            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var result = await _productManager.GetProductAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] ProductWriteDto productWriteDto)
        {
            var result = await _productManager.AddProductAsync(productWriteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] ProductWriteDto productWriteDto)
        {
            var updatedResult = await _productManager.EditProductAsync(id, productWriteDto);

            if (!updatedResult.Success)
            {
                if (updatedResult.Errors != null)
                {
                    return BadRequest(updatedResult);
                }
                else
                {
                    return NotFound(updatedResult);
                }
            }

            return Ok(updatedResult);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _productManager.DeleteProductAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("category/{categoryId:int}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetProductsByCategoryIdAsync(int categoryId)
        {
            var result = await _productManager.GetProductsByCategoryIdAsync(categoryId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("{id:int}/image")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> UploadImageAsync([FromForm] ImageUploadDto imageUploadDto, int id)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;

            var result = await _productManager.UploadProductImageAsync(imageUploadDto, basePath, schema, host, id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}