using System.Linq.Expressions;

namespace JobHub.DataAccess.IRepo
{
    public interface IRepository<T>
    {
        List<T> GetAll(string? includedEntities = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> func, string? includedEntities = null);
        void Add(T item);
        void Delete(T entity);
    }
}
