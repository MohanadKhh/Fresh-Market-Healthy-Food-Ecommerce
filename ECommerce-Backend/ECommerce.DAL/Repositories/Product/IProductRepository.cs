using ECommerce.Common;
using ECommerce.Common.Pagination;

namespace ECommerce.DAL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();
        Task<Product?> GetByIdWithCategoryAsync(int id);
        Task<PaginationResult<Product>> GetAllByPagination(
            PaginationParameters paginationParameters,
            ProductFilter productFilter);
    }
}