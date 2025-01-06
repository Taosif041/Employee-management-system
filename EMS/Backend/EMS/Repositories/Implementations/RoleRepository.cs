using System.Data;
using System.Threading.Tasks;
using Dapper;
using EMS.Repositories.Interfaces;
using EMS.Models;
using EMS.Services.Interfaces;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.Helpers.ErrorHelper;
using EMS.Helpers;
using static EMS.Helpers.Enums;

namespace EMS.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly ApiResultFactory _apiResultFactory;
        private readonly OperationLogger _operationLogger;
        private readonly IOperationLogRepository _operationLogRepository;

        public RoleRepository(
            IDatabaseFactory databaseFactory,
            IOperationLogRepository operationLogRepository,
            ApiResultFactory apiResultFactory)
        {
            _databaseFactory = databaseFactory;
            _apiResultFactory = apiResultFactory;

            _operationLogRepository = operationLogRepository;
            _operationLogger = new OperationLogger(_operationLogRepository);
        }

        public async Task<ApiResult> AssignRoleToUserIdAsync(int userId, int role)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId, DbType.Int32);
                    parameters.Add("@Role", role, DbType.Int32);

                    var result = await connection.ExecuteScalarAsync<int>("AssignRoleToUserId", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Role, null, OperationType.AssignRole);

                    if (result == 1)
                    {
                        return _apiResultFactory.CreateSuccessResult("Role assigned successfully.");
                    }
                    else
                    {
                        return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Failed to assign role", ErrorLayer.Repository);
                    }
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Error assigning role", ErrorLayer.Repository);
                }
            }
        }

        public async Task<ApiResult> GetRoleByUserIdAsync(int userId)
        {
            using (IDbConnection connection = _databaseFactory.CreateSqlServerConnection())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId, DbType.Int32);

                    var role = await connection.ExecuteScalarAsync<int>("GetRoleByUserId", parameters, commandType: CommandType.StoredProcedure);

                    await _operationLogger.LogOperationAsync(EntityName.Role, null, OperationType.GetRole);

                    return _apiResultFactory.CreateSuccessResult(role);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Error retrieving role", ErrorLayer.Repository);
                }
            }
        }
    }
}
