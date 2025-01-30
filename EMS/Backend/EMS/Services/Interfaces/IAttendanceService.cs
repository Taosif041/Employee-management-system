using EMS.DtoMapping.DTOs.Attendance;
using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<ApiResult> GetAllAttendanceAsync();  
        Task<ApiResult> GetAttendanceByAttendanceId(int attendanceId);  
        Task<ApiResult> CreateAttendanceAsync(CreateAttendanceDto dto);  
        Task<ApiResult> UpdateAttendanceAsync(int attendanceId, UpdateAttendanceDto dto);  
        Task<ApiResult> DeleteAttendanceAsync(int attendanceId);  
    }
}
