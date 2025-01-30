using Dapper;
using Microsoft.Data.SqlClient;
using EMS.Models;
using EMS.Repositories.Interfaces;
using System.Data;
using static EMS.Helpers.Enums;
using EMS.DtoMapping.DTOs;
using Employee = EMS.Models.Employee;
using EMS.Helpers;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.EMS.Repositories.DatabaseProviders.Implementations;
using EMS.Helpers.ErrorHelper;
using EMS.DtoMapping;

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
                    var result = await connection.QueryAsync<Employee>("GetAllEmployees", commandType: CommandType.StoredProcedure);


                    await _operationLogger.LogOperationAsync(EntityName.Employee, null, OperationType.GetAll);
                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                        ErrorMessage.GET_EMPLOYEE_ERROR, ErrorLayer.Repository);
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
                    var result = await connection.QueryFirstOrDefaultAsync<Employee>("GetEmployeeById", parameters, commandType: CommandType.StoredProcedure);


                    await _operationLogger.LogOperationAsync(EntityName.Employee, employeeId, OperationType.GetById);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                        ErrorMessage.GET_EMPLOYEE_ERROR, ErrorLayer.Repository);
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
                    var newId = await connection.ExecuteScalarAsync<int>("CreateEmployee", parameters, commandType: CommandType.StoredProcedure);

                    employee.EmployeeId = newId;

                    //var employeeDTO = EmployeeMapper.ToDTO(employee);

                    await _operationLogger.LogOperationAsync(EntityName.Employee, newId, OperationType.Create);

                    return _apiResultFactory.CreateSuccessResult(employee);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_EMPLOYEE_ERROR, ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> UpdateEmployeeInformationAsync(Employee employee)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
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

                    await connection.ExecuteAsync("UpdateEmployeeInformation", parameters, commandType: CommandType.StoredProcedure);

                    //var employeeDTO = EmployeeMapper.ToDTO(employee);

                    await _operationLogger.LogOperationAsync(EntityName.Employee, employee.EmployeeId, OperationType.Update);

                    return _apiResultFactory.CreateSuccessResult(employee);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_EMPLOYEE_ERROR, ErrorLayer.Repository);
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
                    var rowsAffected = await connection.ExecuteAsync("DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Employee, employeeId, OperationType.Delete);

                    return _apiResultFactory.CreateSuccessResult(rowsAffected > 0);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_EMPLOYEE_ERROR, ErrorLayer.Repository);
                }
            }
        }
    }
}
