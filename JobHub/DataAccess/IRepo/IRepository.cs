using System.Linq.Expressions;

namespace JobHub.DataAccess.IRepo
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll(string? includedEntities = null);
        IEnumerable<T> GetRange(Expression<Func<T, bool>> func, string? includedEntities = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> func, string? includedEntities = null);
        void Add(T item);
        void Delete(T entity);
        void Update(T entity);
        IEnumerable<T> ExecuteCustomQuery(Func<IQueryable<T>, IQueryable<T>> func);
    }
}
