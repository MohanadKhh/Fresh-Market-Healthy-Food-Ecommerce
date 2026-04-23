using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return;
            _context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
             => await _context.Set<T>().Where(predicate).ToListAsync();
    }
}
