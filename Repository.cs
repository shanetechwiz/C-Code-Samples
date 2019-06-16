using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EntityContext _context;
        private DbSet<T> _dbSet;

        public Repository(EntityContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> FindAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void InsertRange(IEnumerable<T> entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (entity.Any())
            {
                _dbSet.AddRange(entity);
                _context.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
            _context.SaveChanges();
        }

        public int FindAllThenCount()
        {
            return _dbSet.AsNoTracking().Count();
        }

        public int FindAllWhereThenCount(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Select(predicate).Count();
        }

        public bool ExistAnyWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Any(predicate);
        }

        public bool ExistAnyById(int id)
        {
            return _dbSet.Any(Global.ExpressionBuilder<T>(id));
        }

        public T FindSingleWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().SingleOrDefault(predicate);
        }

        public IEnumerable<T> FindAllWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public IEnumerable<T> FindAllAndInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAllIncluding(includeProperties).ToList();
        }

        public IEnumerable<T> FindWhereAndInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = FindAllIncluding(includeProperties);
            return query.Where(predicate).ToList();
        }

        public T FindByIdAndInclude(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = FindAllIncluding(includeProperties);
            return query.FirstOrDefault(Global.ExpressionBuilder<T>(id));
        }

        public void ExecuteRawSqlQuery(string storedProcedure, params object[] procedureParameters)
        {
            _dbSet.FromSql<T>("EXEC " + storedProcedure, procedureParameters);
        }

        private IQueryable<T> FindAllIncluding(Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = _dbSet.AsNoTracking();
            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
