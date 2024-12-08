using EMS.Models;

namespace EMS.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeInformationAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int employeeId);

    }

}
