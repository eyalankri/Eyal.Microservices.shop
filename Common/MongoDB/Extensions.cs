using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Common.Settings;
using MongoDB.Driver;
using Common.Repositories;

namespace Common.MongoDB
{
    public static class Extensions
    {

        // AddMongo will be initiated from the startup.cs/program.cs of each project who needs it
        // Read the ServiceSetting.cs - this file in each project defines the settings in appsettings.json
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String)); // we dont want to encrypt guid in mongo
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String)); // we dont want to encrypt time

            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();

                // req: dotnet add package Microsoft.Extensions.Configuration.Binder
                var serviceSettings = configuration!.GetSection("ServiceSettings").Get<ServiceSettings>(); //deserialize "ServiceSettings" section from appsettings into to ServiceSettings class 
                var mongoDbSettings = configuration!.GetSection("MongoDbSettings").Get<MongoDbSettings>(); 
                var mongoClient = new MongoClient(mongoDbSettings!.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings!.ServiceName);

            });

            return services;
        }


        // AddMongoRepository will be initiated from the startup.cs/program.cs file of the project how needs this
        // `this` will extend IServiceCollection from the caller project program.cs: `builder.Services.AddMongoRepository<Item>("items")` .Services is IServiceCollection

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
          where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database!, collectionName);
            });

            return services;
        }
 
    }
}
