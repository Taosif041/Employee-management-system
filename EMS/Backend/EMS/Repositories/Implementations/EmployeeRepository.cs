using Dapper;
using Microsoft.Data.SqlClient;
using EMS.Models;
using EMS.Repositories.Interfaces;
using System.Data;
using static EMS.Helpers.Enums;
using EMS.DTOs.Employee;
using Employee = EMS.Models.Employee;
using EMS.Helpers;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.EMS.Repositories.DatabaseProviders.Implementations;
using EMS.Core.Helpers;

namespace EMS.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly ApiResultFactory _apiResultFactory;

        private readonly OperationLogger _operationLogger;
        private readonly IOperationLogRepository _operationLogRepository;

        public EmployeeRepository(IDatabaseFactory databaseFactory, IOperationLogRepository operationLogRepository, ApiResultFactory apiResultFactory)
        {
            _databaseFactory = databaseFactory;
            _apiResultFactory = apiResultFactory;

            _operationLogRepository = operationLogRepository;
            _operationLogger = new OperationLogger(_operationLogRepository);
        }

        public async Task<ApiResult> GetAllEmployeesAsync()
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    // Fetch employees from database
                    var employees = await connection.QueryAsync<Employee>("GetAllEmployees", commandType: CommandType.StoredProcedure);

                    // Map Employee list to EmployeeDTO list
                    var employeeDTOs = EmployeeMapper.ToDTOList(employees.ToList());

                    // Log the operation
                    await _operationLogger.LogOperationAsync(EntityName.Employee, null, OperationType.GetAll);

                    // Return the success result with EmployeeDTO list
                    return _apiResultFactory.CreateSuccessResult(employeeDTOs);
                }
                catch (Exception ex)
                {
                    // Return error if exception occurs
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR);
                }
            }
        }

        public async Task<ApiResult> GetEmployeeByIdAsync(int employeeId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                var parameters = new { EmployeeId = employeeId };
                try
                {
                    // Fetch employee by ID from database
                    var employee = await connection.QueryFirstOrDefaultAsync<Employee>("GetEmployeeById", parameters, commandType: CommandType.StoredProcedure);

                    // Map the Employee to EmployeeDTO
                    var employeeDTO = EmployeeMapper.ToDTO(employee);

                    // Log the operation
                    await _operationLogger.LogOperationAsync(EntityName.Employee, employeeId, OperationType.GetById);

                    // Return the success result with EmployeeDTO
                    return _apiResultFactory.CreateSuccessResult(employeeDTO);
                }
                catch (Exception ex)
                {
                    // Return error if exception occurs
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR);
                }
            }
        }

        public async Task<ApiResult> CreateEmployeeAsync(Employee employee)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
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
                try
                {
                    // Create employee in the database and get the new ID
                    var newId = await connection.ExecuteScalarAsync<int>("CreateEmployee", parameters, commandType: CommandType.StoredProcedure);

                    // Set the new ID for the employee
                    employee.EmployeeId = newId;

                    // Map the created Employee to EmployeeDTO
                    var employeeDTO = EmployeeMapper.ToDTO(employee);

                    // Log the operation
                    await _operationLogger.LogOperationAsync(EntityName.Employee, newId, OperationType.Create);

                    // Return the success result with the created EmployeeDTO
                    return _apiResultFactory.CreateSuccessResult(employeeDTO);
                }
                catch (Exception ex)
                {
                    // Return error if exception occurs
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_EMPLOYEE_ERROR);
                }
            }
        }

        public async Task<ApiResult> UpdateEmployeeInformationAsync(Employee employee)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    // Get the current employee information to use for updates
                    var currentEmployee = await GetEmployeeByIdAsync(employee.EmployeeId);

                    var parameters = new
                    {
                        EmployeeId = employee.EmployeeId,
                        Name = employee.Name ?? currentEmployee.Data.Name,
                        Email = employee.Email ?? currentEmployee.Data.Email,
                        Phone = employee.Phone ?? currentEmployee.Data.Phone,
                        Address = employee.Address ?? currentEmployee.Data.Address,
                        DOB = employee.DOB ?? currentEmployee.Data.DOB,
                        DepartmentId = employee.DepartmentId ?? currentEmployee.Data.DepartmentId,
                        DesignationId = employee.DesignationId ?? currentEmployee.Data.DesignationId
                    };

                    // Update employee information in the database
                    await connection.ExecuteAsync("UpdateEmployeeInformation", parameters, commandType: CommandType.StoredProcedure);

                    // Map the updated Employee to EmployeeDTO
                    var employeeDTO = EmployeeMapper.ToDTO(employee);

                    await _operationLogger.LogOperationAsync(EntityName.Employee, employee.EmployeeId, OperationType.Update);

                    // Return the success result with the updated EmployeeDTO
                    return _apiResultFactory.CreateSuccessResult(employeeDTO);
                }
                catch (Exception ex)
                {
                    // Return error if exception occurs
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_EMPLOYEE_ERROR);
                }
            }
        }

        public async Task<ApiResult> DeleteEmployeeAsync(int employeeId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                var parameters = new { EmployeeId = employeeId };
                try
                {
                    // Delete employee from the database
                    var rowsAffected = await connection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);

                    // Log the operation
                    await _operationLogger.LogOperationAsync(EntityName.Employee, employeeId, OperationType.Delete);

                    // Return the success result with the result of delete operation
                    return _apiResultFactory.CreateSuccessResult(rowsAffected > 0);
                }
                catch (Exception ex)
                {
                    // Return error if exception occurs
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_EMPLOYEE_ERROR);
                }
            }
        }
    }
}
