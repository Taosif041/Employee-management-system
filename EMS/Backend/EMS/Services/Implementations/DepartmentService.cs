using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            try
            {
                var departments = await _departmentRepository.GetAllDepartmentsAsync();

                // Log the departments (for debugging purposes)
                Console.WriteLine("Departments fetched: ");
                //foreach (var department in departments)
                //{
                //    Console.WriteLine($"DepartmentId: {department.DepartmentId}, Name: {department.Name}");
                //}

                return departments;
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error fetching departments: {ex.Message}");

                // Rethrow the exception or handle gracefully
                throw new Exception("An error occurred while retrieving the departments.", ex);
            }
        }

        public async Task<Department> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                return await _departmentRepository.GetDepartmentByIdAsync(departmentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching department by ID: {ex.Message}");
                throw new Exception($"An error occurred while retrieving the department with ID {departmentId}.", ex);
            }
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            try
            {
                if (department == null)
                {
                    throw new ArgumentNullException(nameof(department), "Department cannot be null.");
                }

                return await _departmentRepository.CreateDepartmentAsync(department);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating department: {ex.Message}");
                throw new Exception("An error occurred while creating the department.", ex);
            }
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            try
            {
                if (department == null)
                {
                    throw new ArgumentNullException(nameof(department), "Department cannot be null.");
                }

                return await _departmentRepository.UpdateDepartmentAsync(department);
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("Department cannot be null while updating in service layer.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating department: {ex.Message}");
                throw new Exception("An error occurred while updating the department.", ex);
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                {
                    throw new ArgumentException("Invalid department ID.");
                }

                return await _departmentRepository.DeleteDepartmentAsync(departmentId);
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Invalid department ID for deletion.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting department: {ex.Message}");
                throw new Exception("An error occurred while deleting the department.", ex);
            }
        }
    }
}
