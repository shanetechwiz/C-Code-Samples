using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> FindAll();
        T FindById(int id);
        bool ExistAnyWhere(Expression<Func<T, bool>> predicate);
        bool ExistAnyById(int id);
        IEnumerable<T> FindAllAndInclude(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindWhereAndInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindAllWhere(Expression<Func<T, bool>> predicate);
        T FindByIdAndInclude(int id, params Expression<Func<T, object>>[] includeProperties);
        T FindSingleWhere(Expression<Func<T, bool>> predicate);
        void ExecuteRawSqlQuery(string storedProcedure, params object[] procedureParameters);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
    }
}
