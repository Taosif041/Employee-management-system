using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.Models;
using static EMS.Helpers.Enums;

namespace EMS.Repositories.Interfaces
{
    public interface IOperationLogRepository
    {
        Task<ApiResult> LogOperationAsync(OperationLog log);

        Task<ApiResult> 
            GetLogsWithParametersAsync(
                OperationType? operationType = null,
                EntityName? entityName = null,
                int? entityId = null
            );

        Task<ApiResult> GetAllLogsAsync();
    }
}
