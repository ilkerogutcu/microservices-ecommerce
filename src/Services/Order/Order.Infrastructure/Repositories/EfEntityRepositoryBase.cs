using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces.Repositories;
using Order.Domain.SeedWork;

namespace Order.Infrastructure.Repositories
{
    public class EfEntityRepositoryBase<TEntity, TContext>
        : IEntityRepository<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        public EfEntityRepositoryBase(TContext context)
        {
            Context = context;
        }

        protected TContext Context { get; }


        public TEntity Add(TEntity entity)
        {
            return Context.Add(entity).Entity;
        }

        public TEntity Update(TEntity entity)
        {
            Context.Update(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query.ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefault(expression);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query.AnyAsync();
        }

        public int GetCount(Expression<Func<TEntity, bool>> expression = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query.Count();
        }

        public IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null, Action<Exception> exceptionAction = null)
        {
            try
            {
                using var transaction = Context.Database.BeginTransaction();
                var result = action();
                transaction.Commit();
                successAction?.Invoke();
                return result;
            }
            catch (Exception ex)
            {
                exceptionAction?.Invoke(ex);
                throw;
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await Context.AddAsync(entity);
            return result.Entity;
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query.ToListAsync();
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (includes == null) return query.FirstOrDefaultAsync(expression);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefaultAsync(expression);
        }

        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query.CountAsync();
        }
    }
}