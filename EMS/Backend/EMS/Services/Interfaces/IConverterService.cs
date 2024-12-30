using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface IConverterService
    {
        Task<ApiResult> GetEmployeeCSVAsync();
        Task<ApiResult> GetEmployeeExcelAsync();
        Task<ApiResult> GetAttendanceCSVAsync();
        Task<ApiResult> GetAttendanceExcelAsync();
    }
}
