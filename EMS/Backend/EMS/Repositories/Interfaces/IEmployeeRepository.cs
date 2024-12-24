using EMS.Models;

namespace EMS.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<ApiResult> GetAllEmployeesAsync();
        Task<ApiResult> GetEmployeeByIdAsync(int employeeId);
        Task<ApiResult> CreateEmployeeAsync(Employee employee);
        Task<ApiResult> UpdateEmployeeInformationAsync(Employee employee);
        Task<ApiResult> DeleteEmployeeAsync(int employeeId);

    }

}
