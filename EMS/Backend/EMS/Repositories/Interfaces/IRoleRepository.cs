using EMS.Models;

namespace EMS.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<ApiResult> AssignRoleToUserIdAsync(int userId, int role);
        Task<ApiResult> GetRoleByUserIdAsync(int userId);

    }
}
