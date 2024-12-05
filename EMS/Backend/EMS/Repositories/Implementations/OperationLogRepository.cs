using EMS.Models;
using EMS.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Repositories.Implementations
{
    public class OperationLogRepository : IOperationLogRepository
    {
        private readonly IMongoCollection<OperationLog> _operationLogCollection;

        public OperationLogRepository(IMongoDatabase database)
        {
            _operationLogCollection = database.GetCollection<OperationLog>("operation_logs");
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LogOperationAsync: {ex.Message}");
                throw; // Rethrow exception to propagate it to the caller
            }
        }

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
                throw; // Rethrow exception to propagate it to the caller
            }
        }

        public async Task<IEnumerable<OperationLog>> GetEmployeeLogsAsync()
        {
            try
            {
                var logs = await _operationLogCollection.FindAsync(log => log.EntityName == "Employee");
                return logs.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeeLogsAsync: {ex.Message}");
                throw; // Rethrow exception to propagate it to the caller
            }
        }

        public async Task<IEnumerable<OperationLog>> GetLogsByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var logs = await _operationLogCollection.FindAsync(log =>
                    log.EntityName == "Employee" && log.EntityId == employeeId);
                return logs.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLogsByEmployeeIdAsync for EmployeeId {employeeId}: {ex.Message}");
                throw; // Rethrow exception to propagate it to the caller
            }
        }
    }
}
