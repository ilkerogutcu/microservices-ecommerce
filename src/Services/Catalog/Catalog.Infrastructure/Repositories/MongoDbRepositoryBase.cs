using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Catalog.Application.Models.Configs;
using Catalog.Domain.Common;
using Catalog.Domain.Utilities.Messages;
using Catalog.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Olcsan.Boilerplate.Utilities.IoC;

namespace Catalog.Infrastructure.Repositories
{
    public class MongoDbRepositoryBase<T> : IDocumentDbRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly ICatalogContext<T> _context;

        public MongoDbRepositoryBase(ICatalogContext<T> context)
        {
            _context = context;
            _collection = context.GetCollection();
        }
        public void Add(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            _collection.InsertOne(entity, options);
        }

        public void AddMany(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options);
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

        public virtual void Update(string id, T record)
        {
            _collection.FindOneAndReplace(x => x.Id == id, record);
        }

        public virtual void Update(T record, Expression<Func<T, bool>> predicate)
        {
            _collection.FindOneAndReplace(predicate, record);
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

        public async Task AddAsync(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            await _collection.InsertOneAsync(entity, options);
        }

        public async Task AddManyAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            await _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options);
        }

        public async Task UpdateAsync(string id, T record)
        {
            await _collection.FindOneAndReplaceAsync(x => x.Id == id, record);
        }

        public async Task UpdateAsync(T record, Expression<Func<T, bool>> predicate)
        {
            await _collection.FindOneAndReplaceAsync(predicate, record);
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