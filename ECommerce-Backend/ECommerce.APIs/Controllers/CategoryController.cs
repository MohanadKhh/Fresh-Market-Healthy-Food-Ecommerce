using ECommerce.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(ICategoryManager categoryManager, IWebHostEnvironment webHostEnvironment)
        {
            _categoryManager = categoryManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryManager.GetAllCategoriesAsync();

            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var result = await _categoryManager.GetCategoryAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CategoryWriteDto categoryWriteDto)
        {
            var result = await _categoryManager.AddCategoryAsync(categoryWriteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] CategoryWriteDto categoryWriteDto)
        {
            var updatedResult = await _categoryManager.EditCategoryAsync(id, categoryWriteDto);

            if (!updatedResult.Success)
            {
                if(updatedResult.Errors != null)
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
            var result = await _categoryManager.DeleteCategoryAsync(id);

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

            var result = await _categoryManager.UploadCategoryImageAsync(imageUploadDto, basePath, schema, host, id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
