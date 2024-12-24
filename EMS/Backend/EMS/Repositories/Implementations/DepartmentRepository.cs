using Dapper;
using Microsoft.Data.SqlClient;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using static EMS.Helpers.Enums;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.Core.Helpers;

namespace EMS.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly OperationLogger _operationLogger;
        private readonly ApiResultFactory _apiResultFactory;
        public DepartmentRepository(IDatabaseFactory databaseFactory, 
            IOperationLogRepository operationLogRepository, ApiResultFactory apiResultFactory)
        {
            _databaseFactory = databaseFactory;
            _operationLogger = new OperationLogger(operationLogRepository);
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> GetAllDepartmentsAsync()
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var result = await connection.QueryAsync<Department>("GetAllDepartments", commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Department, 0, OperationType.GetAll);

                    return _apiResultFactory.CreateSuccessResult(result);
                }

                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR);
                }
            }
        }

        public async Task<ApiResult> GetDepartmentByIdAsync(int departmentId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var parameters = new { DepartmentId = departmentId };
                    var result = await connection.QueryFirstOrDefaultAsync<Department>("GetDepartmentById", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Department, departmentId, OperationType.GetById);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR);
                }
            }
        }

        public async Task<ApiResult> CreateDepartmentAsync(Department department)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {

                try
                {

                    var parameters = new
                    {
                        department.Name
                    };

                    var newId = await connection.ExecuteScalarAsync<int>("CreateDepartment", parameters, commandType: CommandType.StoredProcedure);
                    department.DepartmentId = newId;

                    await _operationLogger.LogOperationAsync(EntityName.Department, newId, OperationType.Create);

                    return _apiResultFactory.CreateSuccessResult(department);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_DEPARTMENT_ERROR);
                }
            }
        }

        public async Task<ApiResult> UpdateDepartmentAsync(Department department)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {

                try
                {
                    var currentDepartment = await GetDepartmentByIdAsync(department.DepartmentId);

                    var parameters = new
                    {
                        DepartmentId = department.DepartmentId,
                        Name = department.Name ?? currentDepartment.Data.Name
                    };

                    var result = await connection.ExecuteAsync("UpdateDepartment", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Department, department.DepartmentId, OperationType.Update);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DEPARTMENT_ERROR);
                }
            }
        }

        public async Task<ApiResult> DeleteDepartmentAsync(int departmentId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {

                try
                {
                    var parameters = new { DepartmentId = departmentId };

                    var rowsAffected = await connection.ExecuteAsync("DeleteDepartment", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Department, departmentId, OperationType.Delete);

                    var result = rowsAffected > 0;
                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DEPARTMENT_ERROR);
                }
            }
        }
    }
}
