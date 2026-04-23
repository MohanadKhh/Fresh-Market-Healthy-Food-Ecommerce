using ECommerce.Common;
using FluentValidation;
using Microsoft.Extensions.Hosting;

namespace ECommerce.BLL
{
    public class ImageManager : IImageManager
    {
        private readonly IValidator<ImageUploadDto> _uploadImageValidator;

        public ImageManager(IValidator<ImageUploadDto> uploadImageValidator)
        {
            _uploadImageValidator = uploadImageValidator;
        }

        public async Task<GeneralResult<ImageUploadResultDto>> UploadImageAsync(
            ImageUploadDto imageUploadDto,
            string? basePath,
            string? schema,
            string? host,
            bool isProductImg = false,
            bool isCategoryImg = false
            )
        {
            if (string.IsNullOrWhiteSpace(schema) || string.IsNullOrWhiteSpace(host))
            {
                return GeneralResult<ImageUploadResultDto>.FailedResult("schema or host is missing");
            }

            var validationResult = await _uploadImageValidator.ValidateAsync(imageUploadDto);
            if (!validationResult.IsValid)
            {
                Dictionary<string, List<Error>> errors = validationResult.ToError();
                return GeneralResult<ImageUploadResultDto>.FailedResult(errors);
            }

            //defint image file name with same extension
            var cleanName = Path.GetFileNameWithoutExtension(imageUploadDto.ImageFile.FileName).Replace(" ", "-").ToLower();
            var ext = Path.GetExtension(imageUploadDto.ImageFile.FileName).ToLower();
            var uniqueFileName = $"{cleanName}-{Guid.NewGuid().ToString()}{ext}";

            //define folder path
            string folderPath = string.Empty;
            if (isProductImg)
            {
                folderPath = Path.Combine(basePath, "Files", "Images", "Products");
            }
            else if (isCategoryImg)
            {
                folderPath = Path.Combine(basePath, "Files", "Images", "Categories");
            }
            else
            {
                folderPath = Path.Combine(basePath, "Files", "Images", "Others");
            }

            //creating folder if not exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //adding image in folder
            using (var stream = new FileStream(Path.Combine(folderPath, uniqueFileName), FileMode.Create))
            {
                await imageUploadDto.ImageFile.CopyToAsync(stream);
            }

            var url = string.Empty;
            if (isProductImg)
            {
                url = $"{schema}://{host}/Files/Images/Products/{uniqueFileName}";
            }
            else if (isCategoryImg)
            {
                url = $"{schema}://{host}/Files/Images/Categories/{uniqueFileName}";
            }
            else
            {
                url = $"{schema}://{host}/Files/Images/Others/{uniqueFileName}";
            }
            var imageUploadResultDto = new ImageUploadResultDto(url);

            return GeneralResult<ImageUploadResultDto>.SuccessedResult(imageUploadResultDto);
        }

        public bool TryDeleteImageFromDisk(string? imageUrl, string basePath, bool isProductImg = false, bool isCategoryImg = false)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return false;

            string relativePath;
            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
                relativePath = uri.AbsolutePath;
            else
                relativePath = imageUrl;

            if (isProductImg && !relativePath.StartsWith("/Files/Images/Products", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (isCategoryImg && !relativePath.StartsWith("/Files/Images/Categories", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (!isProductImg && !isCategoryImg && !relativePath.StartsWith("/Files/Images/Others", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var fileName = Path.GetFileName(relativePath);
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var fullPath = string.Empty;
            if (isProductImg)
            {
                fullPath = Path.Combine(basePath, "Files", "Images", "Products", fileName);
            }
            else if (isCategoryImg)
            {
                fullPath = Path.Combine(basePath, "Files", "Images", "Categories", fileName);
            }
            else
            {
                fullPath = Path.Combine(basePath, "Files", "Images", "Others", fileName);
            }

            if (!File.Exists(fullPath))
                return false;

            File.Delete(fullPath);
            return true;
        }
    }
}
