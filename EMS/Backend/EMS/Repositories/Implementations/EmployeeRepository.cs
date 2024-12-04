using Dapper;
using Microsoft.Data.SqlClient;
using EMS.Models;
using EMS.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;

namespace EMS.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SqlConnection _connection;

        public EmployeeRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                await _connection.OpenAsync();
                return await _connection.QueryAsync<Employee>("GetAllEmployees", commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {

                // Optionally, you can throw the error again to be handled at a higher level
                throw new Exception("An error occurred while fetching the employees from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                // Log any other general exception
                Console.WriteLine($"General Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            await _connection.OpenAsync();
            var parameters = new { EmployeeId = employeeId };
            return await _connection.QueryFirstOrDefaultAsync<Employee>("GetEmployeeById", parameters, commandType: CommandType.StoredProcedure);

        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
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

            var newId = await _connection.ExecuteScalarAsync<int> ("CreateEmployee", parameters, commandType : CommandType.StoredProcedure);

            Console.WriteLine(newId);
            employee.EmployeeId = newId;
            Console.WriteLine(employee.EmployeeId);

            return employee;
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

                //LogEmployeeDetails("Current Employee Details:", currentEmployee);

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

                //LogEmployeeDetails("Updated Parameters:", parameters);

                // Call the stored procedure to update the employee information
                await _connection.ExecuteAsync("UpdateEmployeeInformation", parameters, commandType: CommandType.StoredProcedure);

                // Return the updated employee details
                //return await GetEmployeeByIdAsync(employee.EmployeeId);
                return employee;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("An error occurred while updating employee information in the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while updating employee information.", ex);
            }
        }

        //private void LogEmployeeDetails(string message, object details)
        //{
        //    Console.WriteLine(message);
        //    foreach (var property in details.GetType().GetProperties())
        //    {
        //        var value = property.GetValue(details) ?? "NULL";
        //        Console.WriteLine($"{property.Name}: {value}");
        //    }
        //    Console.WriteLine();
        //}

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new { EmployeeId = employeeId };

                var rowsAffected = await _connection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);

                // Return true if the delete was successful (i.e., rowsAffected > 0)
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
