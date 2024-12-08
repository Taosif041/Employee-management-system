using Dapper;
using Microsoft.Data.SqlClient;
using EMS.Models;
using EMS.Repositories.Interfaces;
using System.Data;
using static EMS.Data.Enums;
using EMS.DTOs.Employee;
using Employee = EMS.Models.Employee;
using EMS.Helpers;

namespace EMS.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SqlConnection _connection;
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly OperationLogger _operationLogger;

        public EmployeeRepository(SqlConnection connection, IOperationLogRepository operationLogRepository)
        {
            _connection = connection;
            _operationLogRepository = operationLogRepository;
            _operationLogger = new OperationLogger(_operationLogRepository);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                await _connection.OpenAsync();
                var employees = await _connection.QueryAsync<Employee>("GetAllEmployees", commandType: CommandType.StoredProcedure);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Employee, null, OperationType.GetAll);

                return employees;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("An error occurred while fetching the employees from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new { EmployeeId = employeeId };

                var employee = await _connection.QueryFirstOrDefaultAsync<Employee>("GetEmployeeById", parameters, commandType: CommandType.StoredProcedure);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Employee, employeeId, OperationType.GetById);

                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while retrieving employee information.", ex);
            }
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new
                {
                    employee.OfficeEmployeeId,
                    employee.Name,
                    employee.Email,
                    employee.Phone,
                    employee.Address,
                    employee.DOB,
                    employee.DepartmentId,
                    employee.DesignationId
                };

                var newId = await _connection.ExecuteScalarAsync<int>("CreateEmployee", parameters, commandType: CommandType.StoredProcedure);

                employee.EmployeeId = newId;

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Employee, newId, OperationType.Create);

                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while creating employee information.", ex);
            }
        }

        public async Task<Employee> UpdateEmployeeInformationAsync(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "Employee object cannot be null.");

            try
            {
                // Retrieve the current employee data from the database
                var currentEmployee = await GetEmployeeByIdAsync(employee.EmployeeId);

                if (currentEmployee == null)
                    throw new Exception($"Employee with ID {employee.EmployeeId} not found.");

                // Prepare the parameters for the update procedure, using current values if input is null
                var parameters = new
                {
                    EmployeeId = employee.EmployeeId,
                    Name = employee.Name ?? currentEmployee.Name,
                    Email = employee.Email ?? currentEmployee.Email,
                    Phone = employee.Phone ?? currentEmployee.Phone,
                    Address = employee.Address ?? currentEmployee.Address,
                    DOB = employee.DOB ?? currentEmployee.DOB,
                    DepartmentId = employee.DepartmentId ?? currentEmployee.DepartmentId,
                    DesignationId = employee.DesignationId ?? currentEmployee.DesignationId
                };

                // Call the stored procedure to update the employee information
                await _connection.ExecuteAsync("UpdateEmployeeInformation", parameters, commandType: CommandType.StoredProcedure);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Employee, employee.EmployeeId, OperationType.Update);

                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while updating employee information.", ex);
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new { EmployeeId = employeeId };

                var rowsAffected = await _connection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Employee, employeeId, OperationType.Delete);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error deleting employee: {ex.Message}");
                throw new Exception("An error occurred while deleting employee information.");
            }
        }
    }
}
