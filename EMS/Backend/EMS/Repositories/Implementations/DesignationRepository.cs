using Dapper;
using Microsoft.Data.SqlClient;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using static EMS.Helpers.Enums;
using EMS.Helpers.ErrorHelper;

namespace EMS.Repositories.Implementations
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly OperationLogger _operationLogger;
        private readonly ApiResultFactory _apiResultFactory;

        public DesignationRepository(IDatabaseFactory databaseFactory,
            IOperationLogRepository operationLogRepository, ApiResultFactory apiResultFactory)
        {
            _databaseFactory = databaseFactory;
            _operationLogger = new OperationLogger(operationLogRepository);
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> GetAllDesignationsAsync()
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var result = await connection.QueryAsync<Designation>("GetAllDesignations", commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Designation, 0, OperationType.GetAll);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR);
                }
            }
        }

        public async Task<ApiResult> GetDesignationByIdAsync(int designationId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var parameters = new { DesignationId = designationId };
                    var result = await connection.QueryFirstOrDefaultAsync<Designation>("GetDesignationById", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Designation, designationId, OperationType.GetById);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR);
                }
            }
        }

        public async Task<ApiResult> CreateDesignationAsync(Designation designation)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var parameters = new { designation.Name };

                    var newId = await connection.ExecuteScalarAsync<int>("CreateDesignation", parameters, commandType: CommandType.StoredProcedure);
                    designation.DesignationId = newId;

                    await _operationLogger.LogOperationAsync(EntityName.Designation, newId, OperationType.Create);

                    return _apiResultFactory.CreateSuccessResult(designation);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_DESIGNATION_ERROR);
                }
            }
        }

        public async Task<ApiResult> UpdateDesignationAsync(Designation designation)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var currentDesignation = await GetDesignationByIdAsync(designation.DesignationId);

                    var parameters = new
                    {
                        DesignationId = designation.DesignationId,
                        Name = designation.Name ?? currentDesignation.Data.Name
                    };

                    var result = await connection.ExecuteAsync("UpdateDesignation", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Designation, designation.DesignationId, OperationType.Update);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DESIGNATION_ERROR);
                }
            }
        }

        public async Task<ApiResult> DeleteDesignationAsync(int designationId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var parameters = new { DesignationId = designationId };

                    var rowsAffected = await connection.ExecuteAsync("DeleteDesignation", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Designation, designationId, OperationType.Delete);

                    var result = rowsAffected > 0;
                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DESIGNATION_ERROR);
                }
            }
        }
    }
}
