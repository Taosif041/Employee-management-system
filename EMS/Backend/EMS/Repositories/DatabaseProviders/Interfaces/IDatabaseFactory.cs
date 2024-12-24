using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using MongoDB.Driver;
using System.Data;

namespace EMS.EMS.Repositories.DatabaseProviders.Interfaces
{
    public interface IDatabaseFactory
    {
        IDbConnection CreateSqlServerConnection();
        IDbConnection CreatePostgresSqlConnection();
        IMongoDatabase CreateMongoDbConnection();

    }
}
