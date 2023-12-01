using Common.Repositories;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Common.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {

        private readonly IMongoCollection<T> dbCollection; // req: dotnet add package MongoDB.Driver
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;


        public MongoRepository(IMongoDatabase database, string collectionName)
        {

            dbCollection = database.GetCollection<T>(collectionName);
        }


        public async  Task<T> CreateAsync(T entity)
        {
            await dbCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync(Guid id)
        {
           var filter = filterBuilder.Where(x=>x.Id == id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            // filter:send linq expression
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
