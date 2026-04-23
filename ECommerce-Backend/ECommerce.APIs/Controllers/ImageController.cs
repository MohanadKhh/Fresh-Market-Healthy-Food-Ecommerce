using ECommerce.BLL;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageController(IImageManager imageManager, IWebHostEnvironment webHostEnvironment)
        {
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync([FromForm] ImageUploadDto imageUploadDto)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;

            var result = await _imageManager.UploadImageAsync(imageUploadDto, basePath, schema, host);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
