using EMS.Models;
using static EMS.Data.Enums;

namespace EMS.Services.Interfaces
{
    public interface IOperationLogService
    {


        Task<IEnumerable<OperationLog>> GetAllLogsAsync();

        // Method to get logs with optional filters (OperationType, EntityName, EntityId)
        Task<IEnumerable<OperationLog>> GetLogsWithParametersAsync(OperationType? operationType = null,
                                                                  EntityName? entityName = null,
                                                                  int? entityId = null);

    }
}
