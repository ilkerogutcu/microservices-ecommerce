﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.Domain.Common
{
    public interface IDocumentDbRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        void AddMany(IEnumerable<T> entities);
        IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> predicate = null);
        void Update(string id, T record);
        void Update(T record, Expression<Func<T, bool>> predicate);
        void DeleteById(string id);
        void Delete(T record);
        bool Any(Expression<Func<T, bool>> predicate = null);
        Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> GetByIdAsync(string id);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null);

        Task AddAsync(T entity);
        Task AddManyAsync(IEnumerable<T> entities);
        Task UpdateAsync(string id, T record);
        Task UpdateAsync(T record, Expression<Func<T, bool>> predicate);
        Task DeleteByIdAsync(string id);
        Task DeleteAsync(T record);
    }
}