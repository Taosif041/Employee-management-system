using Dapper;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Repositories.Interfaces;
using static EMS.Helpers.Enums;
using System.Data;

namespace EMS.Repositories.Implementations
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly OperationLogger _operationLogger;
        private readonly ApiResultFactory _apiResultFactory;
        public RefreshTokenRepository(
            IDatabaseFactory databaseFactory,
            IOperationLogRepository operationLogRepository,
            ApiResultFactory apiResultFactory)
        {

            _databaseFactory = databaseFactory;
            _operationLogger = new OperationLogger(operationLogRepository);
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> AddRefreshTokenAsyc(int userId, string refreshToken)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@UserId", userId);
                parameters.Add("@Token", refreshToken);

                try
                {

                    var newId = await connection.ExecuteScalarAsync<int>("AddRefreshToken", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.RefreshToken, newId, OperationType.Create);

                    return _apiResultFactory.CreateSuccessResult(newId);

                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.ADD_TOKEN_ERROR, ErrorLayer.Repository);
                }
            }
        }



        public async Task<ApiResult> UpdateRefreshTokenAsyc(int userId, string refreshToken)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@userId", userId);
                parameters.Add("@NewToken", refreshToken);
                 

                try
                {
                    var result = await connection.ExecuteAsync("UpdateRefreshToken", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.RefreshToken, 0, OperationType.Update);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_TOKEN_ERROR, ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> GetRefreshTokenByUserIdAsync(int userId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@UserId", userId);
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<RefreshToken>("GetRefreshTokenByUserId", parameters, commandType: CommandType.StoredProcedure);


                    await _operationLogger.LogOperationAsync(EntityName.RefreshToken, result.RefreshTokenId, OperationType.GetById);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_TOKEN_ERROR, ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> GetUserIdByRefreshTokenAsync(string token)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@Token", token);
                try
                {
                    var userId = await connection.QueryFirstOrDefaultAsync<int>("GetUserIdByRefreshToken", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.RefreshToken, userId, OperationType.GET);

                    return _apiResultFactory.CreateSuccessResult(userId);
                    
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_TOKEN_ERROR, ErrorLayer.Repository);
                }
            }
        }





        public async Task<ApiResult> DeleteRefreshTokenAsyc(int userId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@UserId", userId);
                try
                {

                    var count = await connection.ExecuteAsync("DeleteRefreshToken", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.RefreshToken, 0, OperationType.Delete);

                    return _apiResultFactory.CreateSuccessResult(count>0);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_TOKEN_ERROR, ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> DeleteAllExpiredTokensAsync()
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                try
                {

                    var result = await connection.ExecuteAsync("DeleteExpiredRefreshTokens", commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.RefreshToken, 0, OperationType.Delete);

                    return _apiResultFactory.CreateSuccessResult(result>0);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_TOKEN_ERROR, ErrorLayer.Repository);
                }
            }
        }
    }
}
