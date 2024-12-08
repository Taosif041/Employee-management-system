using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.Models;
using static EMS.Data.Enums;

namespace EMS.Repositories.Interfaces
{
    public interface IOperationLogRepository
    {

        Task LogOperationAsync(OperationLog log);
        Task<IEnumerable<OperationLog>> GetLogsWithParametersAsync(OperationType? operationType = null,
                                                           EntityName? entityName = null,
                                                           int? entityId = null);

        Task<IEnumerable<OperationLog>> GetAllLogsAsync();
    }
}
