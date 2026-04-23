namespace ECommerce.BLL
{
    public interface IImageManager
    {
        bool TryDeleteImageFromDisk(string? imageUrl, string basePath, bool isProductImg = false, bool isCategoryImg = false);
        Task<GeneralResult<ImageUploadResultDto>> UploadImageAsync(ImageUploadDto imageUploadDto, string? basePath
                                                                , string? schema, string? host
                                                                , bool isProductImg = false, bool isCategoryImg = false);
    }
}