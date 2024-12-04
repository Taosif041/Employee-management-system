using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;

namespace EMS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        public readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync(); // Assuming this returns Task<List<Employee>>

                // Log the employees (for debugging purposes)
                Console.WriteLine("Employees fetched: ");
                //foreach (var employee in employees)
                //{
                //    Console.WriteLine($"EmployeeId: {employee.EmployeeId}, Name: {employee.Name}");
                //}

                return employees; // No need for casting, as List<Employee> is already IEnumerable<Employee>
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error fetching employees: {ex.Message}");

                // Rethrow the exception if needed, or handle gracefully
                throw new Exception("An error occurred while retrieving the employees.", ex);
            }
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        }
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    throw new ArgumentNullException(nameof(employee), "EMS-employee-service: employee cannot be null");
                }

                return await _employeeRepository.CreateEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                // Log.Error(ex, "An error occurred while creating the employee.");

                // Re-throw the exception or throw a custom exception
                throw new Exception("An error occurred while creating the employee.", ex);
            }
        }

        public async Task<Employee> UpdateEmployeeInformationAsync(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    throw new ArgumentNullException(nameof(employee), "EMS-employee-service: employee cannot be null");
                }

                // Assuming the repository method to update employee information.
                var hello =  await _employeeRepository.UpdateEmployeeInformationAsync(employee);

                

                return hello;
            }
            catch (ArgumentNullException ex)
            {
                // Handle specific null argument case
                throw new Exception("Employee cannot be null while updating in service layer.", ex);
            }
            catch (Exception ex)
            {
                // General error handling
                // Log the error (optional)
                // Log.Error(ex, "An error occurred while updating employee information.");
                Console.WriteLine("\njoy bangla service\n, ",ex.Message);

                throw new Exception("An error occurred while updating employee information -> service layer.", ex);
            }
        }
        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                {
                    throw new ArgumentException("EMS-employee-service: invalid employee ID.");
                }

                // Assuming the repository method to delete the employee.
                return await _employeeRepository.DeleteEmployeeAsync(employeeId);
            }
            catch (ArgumentException ex)
            {
                // Handle specific invalid argument case
                throw new Exception("Invalid employee ID for deletion.", ex);
            }
            catch (Exception ex)
            {
                // General error handling
                // Log the error (optional)
                // Log.Error(ex, "An error occurred while deleting employee information.");

                throw new Exception("An error occurred while deleting employee information.", ex);
            }
        }






    }
}
