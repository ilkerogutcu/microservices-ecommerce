using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Common;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Catalog.Infrastructure.Repositories
{
    public class MongoDbRepositoryBase<T> : IDocumentDbRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> Collection;

        public MongoDbRepositoryBase(ICatalogContext<T> context)
        {
            Collection = context.GetCollection();
        }

        public T Add(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            Collection.InsertOne(entity, options);
            return entity;
        }

        public IEnumerable<T> AddMany(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options);
            return entities;
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? Collection.AsQueryable()
                : Collection.AsQueryable().Where(predicate);
        }

        public T GetById(string id)
        {
            return Collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public T Get(Expression<Func<T, bool>> predicate = null)
        {
            return Collection.Find(predicate).FirstOrDefault();
        }

        public virtual T Update(string id, T record)
        {
            Collection.FindOneAndReplace(x => x.Id == id, record);
            return record;
        }

        public virtual T Update(T record, Expression<Func<T, bool>> predicate)
        {
            Collection.FindOneAndReplace(predicate, record);
            return record;
        }

        public void DeleteById(string id)
        {
            Collection.FindOneAndDelete(x => x.Id == id);
        }

        public void Delete(T record)
        {
            Collection.FindOneAndDelete(x => x.Id == record.Id);
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            var data = predicate == null
                ? Collection.AsQueryable().Any()
                : Collection.AsQueryable().Where(predicate).Any();
            return data;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Task.Run(() => predicate == null
                ? Collection.AsQueryable().AnyAsync()
                : Collection.AsQueryable().AnyAsync(predicate));
        }

        public async Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Task.Run(() => predicate == null
                ? Collection.AsQueryable()
                : Collection.AsQueryable().Where(predicate));
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            var result = await Collection.FindAsync(predicate);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            await Collection.InsertOneAsync(entity, options);
            return entity;
        }

        public async Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options);
            return entities;
        }

        public async Task<T> UpdateAsync(string id, T record)
        {
            await Collection.FindOneAndReplaceAsync(x => x.Id == id, record);
            return record;
        }

        public async Task<T> UpdateAsync(T record, Expression<Func<T, bool>> predicate)
        {
            await Collection.FindOneAndReplaceAsync(predicate, record);
            return record;
        }

        public async Task DeleteByIdAsync(string id)
        {
            await Collection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(T record)
        {
            await Collection.FindOneAndDeleteAsync(x => x.Id == record.Id);
        }
    }
}