using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Identity.Domain.Common;

namespace Identity.Application.Interfaces.Repositories
{
    public interface IEntityRepository<T> where T : class
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        List<T> GetList(Expression<Func<T, bool>> expression = null);
        T Get(Expression<Func<T, bool>> expression);
        int SaveChanges();
        int GetCount(Expression<Func<T, bool>> expression = null);
        IQueryable<T> Query();
        Task<int> Execute(FormattableString interpolatedQueryString);

        TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null,
            Action<Exception> exceptionAction = null);

        Task<T> AddAsync(T entity);
        Task<int> SaveChangesAsync();
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression = null);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<int> GetCountAsync(Expression<Func<T, bool>> expression = null);
    }
}