using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Catalog.Domain.Common
{
    public interface IDocumentDbRepository<T> where T : BaseEntity
    {
        T Add(T entity);
        IEnumerable<T> AddMany(IEnumerable<T> entities);
        IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> predicate = null);
        T Update(string id, T record);
        T Update(T record, Expression<Func<T, bool>> predicate);
        void DeleteById(string id);
        void Delete(T record);
        bool Any(Expression<Func<T, bool>> predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null);

        Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> GetByIdAsync(string id);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null);

        Task<T> AddAsync(T entity);
        
        Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(string id, T record);
        Task<T> UpdateAsync(T record, Expression<Func<T, bool>> predicate);
        Task DeleteByIdAsync(string id);
        Task DeleteAsync(T record);
    }
}