using System.Linq.Expressions;

namespace webapibasica.Infrastructure
{
    public interface IRepository<T> where T : class, new()
    {
        Task<T> GetById(string Id);
        Task<T> Find(Expression<Func<T, bool>> predicade);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicade);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
    }
}