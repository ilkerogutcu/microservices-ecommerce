using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Catalog.Domain.Common;
using Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class MongoDbRepositoryBase<T> : IDocumentDbRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoDbRepositoryBase(ICatalogContext<T> context)
        {
            _collection = context.GetCollection();
        }

        public T Add(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            _collection.InsertOne(entity, options);
            return entity;
        }

        public IEnumerable<T> AddMany(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options);
            return entities;
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? _collection.AsQueryable()
                : _collection.AsQueryable().Where(predicate);
        }

        public T GetById(string id)
        {
            return _collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public T Get(Expression<Func<T, bool>> predicate = null)
        {
            return _collection.Find(predicate).FirstOrDefault();
        }

        public virtual T Update(string id, T record)
        {
            _collection.FindOneAndReplace(x => x.Id == id, record);
            return record;
        }

        public virtual T Update(T record, Expression<Func<T, bool>> predicate)
        {
            _collection.FindOneAndReplace(predicate, record);
            return record;
        }

        public void DeleteById(string id)
        {
            _collection.FindOneAndDelete(x => x.Id == id);
        }

        public void Delete(T record)
        {
            _collection.FindOneAndDelete(x => x.Id == record.Id);
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            var data = predicate == null
                ? _collection.AsQueryable().Any()
                : _collection.AsQueryable().Where(predicate).Any();
            return data;
        }

        public async Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Task.Run(() => predicate == null
                ? _collection.AsQueryable()
                : _collection.AsQueryable().Where(predicate));
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            var result = await _collection.FindAsync(predicate);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            await _collection.InsertOneAsync(entity, options);
            return entity;
        }

        public async Task<IEnumerable<T> > AddManyAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            await _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options);
            return entities;
        }

        public async Task<T> UpdateAsync(string id, T record)
        {
            await _collection.FindOneAndReplaceAsync(x => x.Id == id, record);
            return record;
        }

        public async Task<T> UpdateAsync(T record, Expression<Func<T, bool>> predicate)
        {
            await _collection.FindOneAndReplaceAsync(predicate, record);
            return record;
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _collection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(T record)
        {
            await _collection.FindOneAndDeleteAsync(x => x.Id == record.Id);
        }
    }
}