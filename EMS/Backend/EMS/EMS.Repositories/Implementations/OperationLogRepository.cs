using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Models;
using EMS.Repositories.Interfaces;
using static EMS.Data.Enums;

namespace EMS.Repositories.Implementations
{
    public class OperationLogRepository : IOperationLogRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<OperationLog> _operationLogCollection;

        public OperationLogRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase), "Mongo database cannot be null.");
            _operationLogCollection = _mongoDatabase.GetCollection<OperationLog>("operation_logs");
        }
        public async Task LogOperationAsync(OperationLog log)
        {
            try
            {
                if (log == null)
                {
                    throw new ArgumentNullException(nameof(log), "Log cannot be null.");
                }


                await _operationLogCollection.InsertOneAsync(log);
                var mongoLog = new OperationLog(
                    OperationType.GetAll.ToString(),
                    EntityName.Employee.ToString(),
                    0,
                    "Retrieved all employees",
                    DatabaseType.SqlSqlServer.ToString()
                );
                try
                {
                    await LogOperationAsync(log);
                }
                catch (Exception logEx)
                {
                    Console.WriteLine($"Error occurred while {OperationType.GetAll.ToString()} logging operation: {logEx.Message}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LogOperationAsync: {ex.Message}");
                throw;
            }
        }

        // Function to get logs with parameter(s)
        public async Task<IEnumerable<OperationLog>> GetLogsWithParametersAsync(OperationType? operationType = null,
                                                                                 EntityName? entityName = null,
                                                                                 int? entityId = null)
        {
            try
            {
                var filter = Builders<OperationLog>.Filter.Empty;

                // Add filters based on provided parameters
                if (operationType.HasValue)
                {
                    filter &= Builders<OperationLog>.Filter.Eq(log => log.OperationType, operationType.Value.ToString());
                }

                if (entityName.HasValue)
                {
                    filter &= Builders<OperationLog>.Filter.Eq(log => log.EntityName, entityName.Value.ToString());
                }

                if (entityId.HasValue)
                {
                    filter &= Builders<OperationLog>.Filter.Eq(log => log.EntityId, entityId.Value);
                }

                // Fetch the logs based on the dynamic filter
                var logs = await _operationLogCollection.FindAsync(filter);
                return logs.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLogsWithParametersAsync: {ex.Message}");
                throw new Exception("Error occurred while fetching logs with parameters.", ex);
            }
        }

        // Example function to get all logs (no parameters)
        public async Task<IEnumerable<OperationLog>> GetAllLogsAsync()
        {
            try
            {
                var logs = await _operationLogCollection.FindAsync(_ => true);
                return logs.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllLogsAsync: {ex.Message}");
                throw new Exception("Error occurred while fetching all logs.", ex);
            }
        }
    }
}
