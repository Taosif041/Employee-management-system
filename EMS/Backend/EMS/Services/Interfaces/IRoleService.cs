namespace EMS.Services.Interfaces
{
    public interface IRoleService
    {
        Task<int?> GetRoleByUserIdAsync(int userId);
        Task AssignRoleToUserAsync(int userId, int role);

    }
}
