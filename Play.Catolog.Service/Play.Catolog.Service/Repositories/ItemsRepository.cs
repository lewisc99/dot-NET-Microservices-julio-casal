using MongoDB.Driver;
using Play.Catolog.Service.Models;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Play.Catolog.Service.Repositories
{
    public class ItemsRepository:IItemsRepository
    {
        private const string collectionName = "items"; //collection name 
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        

        public ItemsRepository(IMongoDatabase database)
        {
            //var mongoClient = new MongoClient("mongodb://localhost:27017"); //mongo port
            //var database = mongoClient.GetDatabase("Catalog"); //name of database

            dbCollection = database.GetCollection<Item>(collectionName); //collection like table of database
        }


        //should only be read can't be modified
        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
          
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync(); // get all items

        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id); //get by id

            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        
        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }


            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);



        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, id);

            await dbCollection.DeleteOneAsync(filter);

        }

    }
}
