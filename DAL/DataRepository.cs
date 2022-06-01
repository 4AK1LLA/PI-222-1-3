using DAL.Interfaces;
using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

#nullable disable

namespace DAL
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        public readonly AutoShowContext _context;
        public DataRepository(AutoShowContext context)
        {
            _context = context;
        }

        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> FindRangeAsync(Expression<Func<T, bool>> expression)
        {
            var query = _context.Set<T>().Where(expression);

            return await query.ToListAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public bool NotEmpty(int id)
        {
            return _context.Set<T>().Find(id) != null ? true : false;
        }

        public bool Select(int id)
        {
            return _context.Set<T>().Find(id) != null ? true : false;
        }
    }
}
