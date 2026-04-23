using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetWithCartItemsByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetWithCartItemsByIdAsync(int id)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task RemoveAllItemsWithProductIdAsync(int productId)
        {
            await _context.CartItems
                .Where(ci => ci.ProductId == productId)
                .ExecuteDeleteAsync();
        }
    }
}