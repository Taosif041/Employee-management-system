using EMS.Models;
using EMS.Repositories.Interfaces;
using static EMS.Helpers.Enums;

namespace EMS.Helpers
{
    public class OperationLogger
    {
        private readonly IOperationLogRepository _operationLogRepository;

        public OperationLogger(IOperationLogRepository operationLogRepository)
        {
            _operationLogRepository = operationLogRepository;
        }

        public async Task LogOperationAsync(EntityName entityName, int? entityId, OperationType operationType)
        {

            string databaseType = (entityName == EntityName.Attendance) ? "PostgreSql Server" : "Sql Server";

            string description = operationType == OperationType.GetAll
                ? $"Performed {operationType} operation on {entityName}"
                : $"Performed {operationType} operation on {entityName} with ID {entityId}";

            var log = new OperationLog(
                operationType.ToString(),
                entityName.ToString(),
                entityId ?? 0,
                description,
                databaseType
            );

            try
            {
                await _operationLogRepository.LogOperationAsync(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while logging operation: {ex.Message}");
            }
        }
    }
}
