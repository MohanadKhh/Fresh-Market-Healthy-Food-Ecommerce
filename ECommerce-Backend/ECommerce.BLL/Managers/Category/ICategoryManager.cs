using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface ICategoryManager
    {
        Task<GeneralResult<CategoryReadDto>> AddCategoryAsync(CategoryWriteDto categoryWriteDto);
        Task<GeneralResult> DeleteCategoryAsync(int id);
        Task<GeneralResult<CategoryReadDto>> EditCategoryAsync(int id, CategoryWriteDto categoryWriteDto);
        Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategoriesAsync();
        Task<GeneralResult<CategoryReadDto>> GetCategoryAsync(int id);
        Task<GeneralResult> UploadCategoryImageAsync(ImageUploadDto imageUploadDto,
                                                    string? basePath, string? schema,
                                                    string? host, int categoryId);
    }
}