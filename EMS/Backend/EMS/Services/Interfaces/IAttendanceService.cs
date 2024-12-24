using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<ApiResult> GetAllAttendanceAsync();  
        Task<ApiResult> GetAttendanceByEmployeeIdAndDateAsync(int employeeId, DateTime date);  
        Task<ApiResult> CreateAttendanceAsync(Attendance attendance);  
        Task<ApiResult> UpdateAttendanceAsync(Attendance attendance);  
        Task<ApiResult> DeleteAttendanceAsync(int employeeId, DateTime date);  
    }
}
