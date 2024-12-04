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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly SqlConnection _connection;

        public DepartmentRepository(SqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection), "Database connection cannot be null.");
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            try
            {
                await _connection.OpenAsync();
                return await _connection.QueryAsync<Department>("GetAllDepartments", commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("An error occurred while fetching departments from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while fetching departments.", ex);
            }
        }

        public async Task<Department> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new { DepartmentId = departmentId };
                return await _connection.QueryFirstOrDefaultAsync<Department>("GetDepartmentById", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while fetching the department by ID.", ex);
            }
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department), "Department cannot be null.");

            try
            {
                await _connection.OpenAsync();

                var parameters = new
                {
                    department.Name
                };

                var newId = await _connection.ExecuteScalarAsync<int>("CreateDepartment", parameters, commandType: CommandType.StoredProcedure);
                department.DepartmentId = newId;

                return department;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("An error occurred while creating the department in the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while creating the department.", ex);
            }
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department), "Department object cannot be null.");

            try
            {
                // Retrieve the current department data
                var currentDepartment = await GetDepartmentByIdAsync(department.DepartmentId);
                if (currentDepartment == null)
                    throw new Exception($"Department with ID {department.DepartmentId} not found.");

                var parameters = new
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name ?? currentDepartment.Name
                };

                await _connection.ExecuteAsync("UpdateDepartment", parameters, commandType: CommandType.StoredProcedure);

                return department;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("An error occurred while updating department information in the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while updating department information.", ex);
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new { DepartmentId = departmentId };

                var rowsAffected = await _connection.ExecuteAsync("DeleteDepartment", parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("An error occurred while deleting department information from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while deleting department information.", ex);
            }
        }
    }
}
