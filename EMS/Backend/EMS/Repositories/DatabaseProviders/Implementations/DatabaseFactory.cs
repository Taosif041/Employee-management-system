using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace EMS.EMS.Repositories.DatabaseProviders.Implementations
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly IConfiguration _configuration;

        public DatabaseFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateSqlServerConnection()
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("SqlServerConnection"));
            try
            {
                connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                connection.Dispose();
                Console.WriteLine(" \n\n\n\n\n\n sql connection error\n \n\n\n\n\n\n\n");
                throw new InvalidOperationException("Failed to establish SqlServer connection", ex);
            }
        }

        public IDbConnection CreatePostgresSqlConnection()
        {
            var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSQLServerConnection"));
            try
            {
                connection.Open();
                return connection;
            }
            catch (NpgsqlException ex)
            {
                connection.Dispose();
                Console.WriteLine(" \n\n\n\n\n\n postgres connection error\n \n\n\n\n\n\n\n");
                throw new InvalidOperationException("Failed to establish PostgreSQL Server connection", ex);
            }
        }

        public IMongoDatabase CreateMongoDbConnection()
        {
            try
            {
                var client = new MongoClient( _configuration.GetConnectionString("MongoDbConnection"));
                return client.GetDatabase(_configuration["ConnectionStrings:MongoDbDatabaseName"]);

            }
            catch (MongoException ex)
            {
                Console.WriteLine(" \n\n\n\n\n\n mongo connection error\n \n\n\n\n\n\n\n");
                throw new InvalidOperationException("Failed to obtain MongoDB database", ex);
            }
        }

        
    }
}
