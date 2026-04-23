using System.Linq.Expressions;

namespace ECommerce.DAL
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
    }
}