using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

public static class DatabaseConfiguration
{
    public static void AddDatabaseConnections(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnectionString = configuration.GetConnectionString("SqlServerConnection");
        services.AddScoped<SqlConnection>(_ => new SqlConnection(sqlConnectionString));


        var mongoDBConnectionString = configuration.GetConnectionString("MongoDbConnection");
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = new MongoClient(mongoDBConnectionString);
            return client.GetDatabase("OperationLogDB"); 
        });

        var postgreSQLConnectionString = configuration.GetConnectionString("PostgreSQL");
        services.AddScoped<NpgsqlConnection>(_ => new NpgsqlConnection(postgreSQLConnectionString));
    }
}
