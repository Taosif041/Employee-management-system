using EMS.DtoMapping.DTOs.DepartmentDTOs;
using EMS.Models;
using EMS.Models.DTOs;

namespace EMS.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResult> GetAllDepartmentsAsync();
        Task<ApiResult> GetDepartmentByIdAsync(int departmentId);
        Task<ApiResult> CreateDepartmentAsync(CreateDepartmentDto dto);
        Task<ApiResult> UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto dto);
        Task<ApiResult> DeleteDepartmentAsync(int departmentId);

    }
}
