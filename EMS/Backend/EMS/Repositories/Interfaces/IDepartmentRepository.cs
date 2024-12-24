using EMS.Models;

namespace EMS.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<ApiResult> GetAllDepartmentsAsync();
        Task<ApiResult> GetDepartmentByIdAsync(int departmentId);
        Task<ApiResult> CreateDepartmentAsync(Department department);
        Task<ApiResult> UpdateDepartmentAsync(Department department);
        Task<ApiResult> DeleteDepartmentAsync(int departmentId);
    }
}
