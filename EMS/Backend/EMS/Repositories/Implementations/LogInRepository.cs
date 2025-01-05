using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Repositories.Interfaces;
using System.Data;
using static EMS.Helpers.Enums;

namespace EMS.Repositories.Implementations
{
    public class LogInRepository : ILoginRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly OperationLogger _operationLogger;
        private readonly ApiResultFactory _apiResultFactory;
        private readonly ILogger<LogInRepository> _logger;

        public LogInRepository(
            IDatabaseFactory databaseFactory,
            IOperationLogRepository operationLogRepository,
            ApiResultFactory apiResultFactory,
            ILogger<LogInRepository> logger)
        {

            _databaseFactory = databaseFactory;
            _operationLogger = new OperationLogger(operationLogRepository);
            _apiResultFactory = apiResultFactory;
            _logger = logger;
        }


        public async Task<ApiResult> AddUserAsync(User user)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@Username", user.UserName);
                parameters.Add("@Email", user.Email);
                parameters.Add("@HashedPassword", user.HashedPassword);


                try
                {
                    _logger.LogInformation("Adding a new user with Username: {UserName}", user.UserName);

                    var newId = await connection.ExecuteScalarAsync<int>("AddUser", parameters, commandType: CommandType.StoredProcedure);


                    user.UserId = newId;

                    await _operationLogger.LogOperationAsync(EntityName.User, user.UserId, OperationType.Create);

                    _logger.LogInformation("User added successfully with ID: {UserId}", user.UserId);
                    return _apiResultFactory.CreateSuccessResult(user);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while adding a new user with Username: {UserName} in Repository", user.UserName);

                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.REGISTER_USER_ERROR, ErrorLayer.Repository);
                }
            }
        }




        public async Task<ApiResult> UpdateUserAsync(User user)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@UserId", user.UserId);
                parameters.Add("@Username", user.UserName);
                parameters.Add("@Email", user.Email);
                parameters.Add("@HashedPassword", user.HashedPassword);

                try
                {
                    var result = await connection.ExecuteAsync("UpdateUser", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.User, user.UserId, OperationType.Update);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_USER_ERROR, ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> GetUserByEmailAsync(string email)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@Email", email);
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<User>("GetUserByEmail", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.User, result.UserId, OperationType.GetById);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_USER_ERROR, ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> GetUserByIdAsync(int userId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@UserId", userId);
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<User>("GetUserById", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.User, userId, OperationType.GetById);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_USER_ERROR, ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> GetUserByNameAsync(string userName)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@Username", userName);

                try
                {
                    _logger.LogInformation("Fetching user by Username: {UserName}", userName);

                    var result = await connection.QueryFirstOrDefaultAsync<User>("GetUserByName", parameters, commandType: CommandType.StoredProcedure);

                    if (result != null)
                    {
                        await _operationLogger.LogOperationAsync(EntityName.User, result.UserId, OperationType.GetById);
                        _logger.LogInformation("User fetched successfully with Username: {UserName}, UserId: {UserId}", userName, result.UserId);
                    }
                    else
                    {
                        _logger.LogWarning("No user found with Username: {UserName}", userName);
                    }

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching user by Username: {UserName}", userName);
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_USER_ERROR, ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> DeleteUserAsync(int userId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                DynamicParameters parameters = new();
                parameters.Add("@UserId", userId);
                try
                {

                    var status = await connection.ExecuteAsync("DeleteUser", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.User, userId, OperationType.Delete);

                    return _apiResultFactory.CreateSuccessResult(status);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_USER_ERROR, ErrorLayer.Repository);
                }
            }
        }






    }
}
