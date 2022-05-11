using System.Linq.Expressions;
using webapibasica.Data;
using Microsoft.EntityFrameworkCore;

namespace webapibasica.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected readonly BasicaContext _context;

        public Repository(BasicaContext context)
        {
            _context = context;
        }

        public async Task<T> GetById(string ID)
        {
            var result = await _context.Set<T>().FindAsync(ID);
            result = result ?? new T();
            return result;
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicade)
        {
            var result = await _context.Set<T>().FirstOrDefaultAsync(predicade);
            result = result ?? new T();
            return result;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var result = await _context.Set<T>().ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicade)
        {
            var result = await _context.Set<T>().Where(predicade).ToListAsync();
            //result = result ?? new List<T>();
            return result;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        IQueryable<T> IRepository<T>.AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
    }
}