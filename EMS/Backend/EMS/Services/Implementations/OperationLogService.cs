using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class OperationLogService : IOperationLogService
    {
        private readonly IOperationLogRepository _operationLogRepository;

        public OperationLogService(IOperationLogRepository operationLogRepository)
        {
            _operationLogRepository = operationLogRepository;
        }

        public async Task<IEnumerable<OperationLog>> GetAllLogsAsync()
        {
            try
            {
                return await _operationLogRepository.GetAllLogsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllLogsAsync: {ex.Message}");
                throw; // Propagate the exception to the controller for appropriate response
            }
        }

        public async Task<IEnumerable<OperationLog>> GetEmployeeLogsAsync()
        {
            try
            {
                return await _operationLogRepository.GetEmployeeLogsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeeLogsAsync: {ex.Message}");
                throw; // Propagate the exception to the controller
            }
        }

        public async Task<IEnumerable<OperationLog>> GetLogsByEmployeeIdAsync(int employeeId)
        {
            try
            {
                return await _operationLogRepository.GetLogsByEmployeeIdAsync(employeeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLogsByEmployeeIdAsync for employeeId {employeeId}: {ex.Message}");
                throw; // Propagate the exception to the controller
            }
        }
    }
}
