using EMS.Models;
using static EMS.Helpers.Enums;

namespace EMS.Services.Interfaces
{
    public interface IOperationLogService
    {

        Task<ApiResult> GetAllLogsAsync();

        Task<ApiResult> GetLogsWithParametersAsync(OperationType? operationType = null,
                                                                  EntityName? entityName = null,
                                                                  int? entityId = null);

    }
}
