using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.Models;

namespace EMS.Repositories.Interfaces
{
    public interface IOperationLogRepository
    {
        Task LogOperationAsync(OperationLog log);
        Task<IEnumerable<OperationLog>> GetAllLogsAsync();
        Task<IEnumerable<OperationLog>> GetEmployeeLogsAsync();
        Task<IEnumerable<OperationLog>> GetLogsByEmployeeIdAsync(int employeeId);
    }
}
