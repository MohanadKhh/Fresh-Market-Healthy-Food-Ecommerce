

using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Order>> GetAllWithItemsAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdWithItemsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdWithItemsAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}
