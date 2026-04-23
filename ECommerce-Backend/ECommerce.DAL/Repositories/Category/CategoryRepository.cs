using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetAllByProductsAsync()
        {
            return await _context.Categories.Include(c => c.Products).ToListAsync();
        }

        public async Task<Category?> GetByIdWithProductsAsync(int id)
        {
            return await _context.Categories.Include(c => c.Products)
                                    .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}