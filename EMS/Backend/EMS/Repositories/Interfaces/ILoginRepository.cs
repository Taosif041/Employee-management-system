using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using EMS.Models;

namespace EMS.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<ApiResult> AddUserAsync(User user);
        Task<ApiResult> UpdateUserAsync(User user);


        Task<ApiResult> DeleteUserAsync(int userId);
        Task<ApiResult> GetUserByIdAsync(int userId);
        Task<ApiResult> GetUserByNameAsync(string userName);
        Task<ApiResult> GetUserByEmailAsync(string email);

    }
}
