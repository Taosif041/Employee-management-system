using EMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IOperationLogService
    {
        Task<IEnumerable<OperationLog>> GetAllLogsAsync();
        Task<IEnumerable<OperationLog>> GetEmployeeLogsAsync();
        Task<IEnumerable<OperationLog>> GetLogsByEmployeeIdAsync(int employeeId);
    }
}
