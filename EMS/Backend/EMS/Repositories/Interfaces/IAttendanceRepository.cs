using EMS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<ApiResult> GetAllAttendanceAsync();
        Task<ApiResult> GetAttendanceByIdAndDateAsync(int employeeId, DateTime date);
        Task<ApiResult> CreateAttendanceAsync(Attendance attendance);
        Task<ApiResult> UpdateAttendanceAsync(Attendance attendance);
        Task<ApiResult> DeleteAttendanceAsync(int employeeId, DateTime date);
    }
}
