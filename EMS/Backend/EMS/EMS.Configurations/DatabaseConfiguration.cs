using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;

public static class DatabaseConfiguration
{
    public static void AddDatabaseConnections(this IServiceCollection services, IConfiguration configuration)
    {
        // Register SqlConnection with the connection string from configuration
        var sqlConnectionString = configuration.GetConnectionString("SqlServerConnection");
        services.AddScoped<SqlConnection>(_ => new SqlConnection(sqlConnectionString));

        // Register MongoDB services: Get IMongoDatabase from IMongoClient
        var mongoDBConnectionString = configuration.GetConnectionString("MongoDbConnection");
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = new MongoClient(mongoDBConnectionString);
            return client.GetDatabase("OperationLogDB"); // specify the database name
        });
    }
}
