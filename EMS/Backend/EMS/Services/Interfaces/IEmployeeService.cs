using EMS.DtoMapping.DTOs.EmployeeDTOs;
using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResult> GetAllEmployeesAsync();
        Task<ApiResult> GetEmployeeByIdAsync(int employeeId);
        Task<ApiResult> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<ApiResult> UpdateEmployeeInformationAsync(int employeeId, UpdateEmployeeDto dto);
        Task<ApiResult> DeleteEmployeeAsync(int employeeId);
    }

}
