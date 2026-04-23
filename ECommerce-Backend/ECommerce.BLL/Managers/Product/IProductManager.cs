using ECommerce.Common;
using ECommerce.Common.Pagination;
using ECommerce.DAL;

namespace ECommerce.BLL
{
    public interface IProductManager
    {
        Task<GeneralResult<ProductReadDto?>> AddProductAsync(ProductWriteDto productWriteDto);
        Task<GeneralResult> DeleteProductAsync(int id);
        Task<GeneralResult<ProductReadDto?>> EditProductAsync(int id, ProductWriteDto productWriteDto);
        Task<GeneralResult<IEnumerable<ProductReadDto>>> GetAllProductsAsync();
        Task<GeneralResult<PaginationResult<Product>>> GetAllProductsByPaginationAsync(PaginationParameters paginationParameters, ProductFilter productFilter);
        Task<GeneralResult<ProductReadDto?>> GetProductAsync(int id);
        Task<GeneralResult<IEnumerable<ProductReadDto>>> GetProductsByCategoryIdAsync(int categoryId);
        Task<GeneralResult> UploadProductImageAsync(ImageUploadDto imageUploadDto, string? basePath, string? schema, string? host, int productId);
    }
}