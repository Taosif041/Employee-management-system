using EMS.Models;

namespace EMS.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<ApiResult> AddRefreshTokenAsyc(int userId, string refreshToken);

        Task<ApiResult> UpdateRefreshTokenAsyc(int userId, string refreshToken);

        Task<ApiResult> GetRefreshTokenByUserIdAsync(int userId);

        Task<ApiResult> GetUserIdByRefreshTokenAsync(string token);

        Task<ApiResult> DeleteRefreshTokenAsyc(int userId);
        Task<ApiResult> DeleteAllExpiredTokensAsync();
    }
}
