using JobHub.Data;
using JobHub.DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobHub.DataAccess.Repo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> ExecuteCustomQuery(Func<IQueryable<T>, IQueryable<T>> func)
        {
            return func(_dbSet).AsEnumerable();
        }

        public IEnumerable<T> GetAll(string? includedEntities = null)
        {
            IQueryable<T> query = _dbSet;
            query = IncludeEntities(query, includedEntities);
            return query.AsEnumerable();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> func, string? includedEntities = null)
        {
            IQueryable<T> query = _dbSet.Where(func);
            query = IncludeEntities(query, includedEntities);
            return query.FirstOrDefault();

        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // @param entities - list of entities separated with comma to be included in query
        IQueryable<T> IncludeEntities(IQueryable<T> query, string? entities)
        {
            if(entities != null)
            {
                foreach (var ent in entities.Split(','))
                {
                    query = query.Include(ent);
                }
            }
            return query;
        }
    }
}
