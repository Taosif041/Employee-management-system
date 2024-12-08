using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EMS.Data.Enums;

namespace EMS.Services.Implementations
{
    public class OperationLogService : IOperationLogService
    {
        private readonly IOperationLogRepository _operationLogRepository;

        public OperationLogService(IOperationLogRepository operationLogRepository)
        {
            _operationLogRepository = operationLogRepository;
        }

        // Get all logs without any filters
        public async Task<IEnumerable<OperationLog>> GetAllLogsAsync()
        {
            try
            {
                return await _operationLogRepository.GetAllLogsAsync();
            }
            catch (Exception ex)
            {
                // Log the error (use a logging framework in production)
                Console.WriteLine($"Error in GetAllLogsAsync: {ex.Message}");
                throw; // Propagate the exception to the controller
            }
        }

        // Get logs based on filters: operationType, entityName, or entityId
        public async Task<IEnumerable<OperationLog>> GetLogsWithParametersAsync(OperationType? operationType = null,
                                                                                 EntityName? entityName = null,
                                                                                 int? entityId = null)
        {
            try
            {
                return await _operationLogRepository.GetLogsWithParametersAsync(operationType, entityName, entityId);
            }
            catch (Exception ex)
            {
                // Log the error (use a logging framework in production)
                Console.WriteLine($"Error in GetLogsWithParametersAsync: {ex.Message}");
                throw; // Propagate the exception to the controller
            }
        }

        

    }
}
