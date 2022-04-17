using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Order.Domain.SeedWork;

namespace Order.Application.Interfaces.Repositories
{
    public interface IEntityRepository<T> : IRepository<T> where T : BaseEntity
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        List<T> GetList(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includes);
        T Get(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        int SaveChanges();
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression = null);

        int GetCount(Expression<Func<T, bool>> expression = null);
        IQueryable<T> Query();

        TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null,
            Action<Exception> exceptionAction = null);

        Task<T> AddAsync(T entity);
        Task<int> SaveChangesAsync();
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        Task<int> GetCountAsync(Expression<Func<T, bool>> expression = null);
    }
}