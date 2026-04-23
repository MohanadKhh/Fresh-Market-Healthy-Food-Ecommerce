namespace ECommerce.DAL
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllWithItemsAsync();
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task<IEnumerable<Order>> GetByUserIdWithItemsAsync(string userId);
    }
}