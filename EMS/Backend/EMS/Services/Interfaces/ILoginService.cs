using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface ILoginService
    {
        Task<ApiResult> RegisterUserAsync(User user);
        //Task<ApiResult> UpdateUser(User user);
        
        Task<ApiResult> LoginUserAsync(string userName, string plainPassword);

        Task<ApiResult> LogoutUserAsync(int userId);

        Task<ApiResult> RefreshUserAsync(string refreshToken);
    }
}
