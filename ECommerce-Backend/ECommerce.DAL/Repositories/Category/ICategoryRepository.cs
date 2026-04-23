namespace ECommerce.DAL
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllByProductsAsync();
        Task<Category?> GetByIdWithProductsAsync(int id);
    }
}