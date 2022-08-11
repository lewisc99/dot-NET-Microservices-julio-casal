using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catolog.Service.Models;
using Play.Catolog.Service.Settings;

namespace Play.Catolog.Service.Repositories
{
    public static class Extensions
    {

        public static IServiceCollection AddMongo(this IServiceCollection services)
        {

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));


        

            //and that's it so with this uh with this section we have defined a singleton object that represents a an
            //mongo database that's going to be injected as remember into um items repository over here is going to
            //be landing over here i'm on the database so here's where we're constructing and registering it with the service
            /// container and then one more thing that we need here is to uh also in register

            services.AddSingleton(ServiceProvider =>
            {
                var configuration = ServiceProvider.GetService<IConfiguration>();

                 var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });


            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) 
            where T: IEntity
        {

            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                //anytime you need to get a instance of a service already registered in the service container
                //just need to call get service (service provider).
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);

            });

            return services;

        }
    }
}
