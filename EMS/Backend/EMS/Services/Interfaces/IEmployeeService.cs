using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResult> GetAllEmployeesAsync();
        Task<ApiResult> GetEmployeeByIdAsync(int employeeId);
        Task<ApiResult> CreateEmployeeAsync(Employee employee);
        Task<ApiResult> UpdateEmployeeInformationAsync(Employee employee);
        Task<ApiResult> DeleteEmployeeAsync(int employeeId);
    }

}
