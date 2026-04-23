using ECommerce.Common;
using ECommerce.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int categoryId)
        {
            return await _context.Products.Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId).ToListAsync();
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _context.Products.Include(p => p.Category)
                                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PaginationResult<Product>> GetAllByPagination(
            PaginationParameters paginationParameters,
            ProductFilter productFilter)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category);
            if (productFilter != null)
            {
                query = ApplyFilter(query, productFilter);
            }

            var totalCount = await query.CountAsync();
            var pageSize = paginationParameters.PageSize;
            pageSize = Math.Clamp(pageSize, 1, 50);

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pageNumber = paginationParameters.PageNumber;
            pageNumber = Math.Clamp(pageNumber, 1, totalPages);

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResult<Product>
            {
                Data = data,
                Metadata = new PaginationMetadata
                {
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    HasNext = pageNumber < totalPages,
                    HasPrevious = pageNumber > 1,
                }
            };
        }

        private static IQueryable<Product> ApplyFilter(IQueryable<Product> query, ProductFilter productFilter)
        {
            if (productFilter.MaxPrice > 0 && productFilter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= productFilter.MaxPrice);
            }

            if (productFilter.MinPrice > 0 && productFilter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= productFilter.MinPrice);
            }

            if (productFilter.inStockOnly == true)
            {
                query = query.Where(p => p.Stock > 0);
            }

            if (!string.IsNullOrEmpty(productFilter.SearchByName))
            {
                query = query.Where(e => e.Name.Contains(productFilter.SearchByName));
            }

            if (!string.IsNullOrEmpty(productFilter.SortBy))
            {
                if (productFilter.SortBy.ToLower() == "price")
                {
                    if (productFilter.SortDescending)
                        query = query.OrderByDescending(p => p.Price);
                    else
                        query = query.OrderBy(p => p.Price);
                }
                if (productFilter.SortBy.ToLower() == "reviews")
                {
                    if (productFilter.SortDescending)
                        query = query.OrderByDescending(p => p.Reviews);
                    else
                        query = query.OrderBy(p => p.Reviews);
                }
            }
            return query;
        }
    }
}
