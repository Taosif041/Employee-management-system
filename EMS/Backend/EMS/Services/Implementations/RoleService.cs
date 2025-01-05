using System.Data;

namespace EMS.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IDbConnection _dbConnection;

        public RoleService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int?> GetRoleByUserIdAsync(int userId)
        {
            var role = await _dbConnection.QuerySingleOrDefaultAsync<int?>(
                "GetRoleByUserId ",
                new { UserId = userId }
            );
            return role;
        }

        public async Task AssignRoleToUserAsync(int userId, int role)
        {
            await _dbConnection.ExecuteAsync(
                "EXEC AssignRoleToUserId @UserId, @Role",
                new { UserId = userId, Role = role }
            );
        }
    }

}
