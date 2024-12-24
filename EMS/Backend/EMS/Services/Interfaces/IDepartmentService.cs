using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResult> GetAllDepartmentsAsync();
        Task<ApiResult> GetDepartmentByIdAsync(int departmentId);
        Task<ApiResult> CreateDepartmentAsync(Department department);
        Task<ApiResult> UpdateDepartmentAsync(Department department);
        Task<ApiResult> DeleteDepartmentAsync(int departmentId);

    }
}
