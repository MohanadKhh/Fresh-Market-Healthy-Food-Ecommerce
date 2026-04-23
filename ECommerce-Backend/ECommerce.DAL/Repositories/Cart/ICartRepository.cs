namespace ECommerce.DAL
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetWithCartItemsByUserIdAsync(string userId);
        Task<Cart?> GetWithCartItemsByIdAsync(int id);
        Task RemoveAllItemsWithProductIdAsync(int productId);
    }
}