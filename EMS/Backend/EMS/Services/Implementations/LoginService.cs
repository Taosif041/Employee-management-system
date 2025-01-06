using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations
{
    public class LoginService : ILoginService
    {
        private readonly ApiResultFactory _apiResultFactory;
        private readonly ILoginRepository _loginRepository;
        private readonly ILogger<LoginService> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IRoleRepository _roleRepository;

        public LoginService(
            ApiResultFactory apiResultFactory,
            ILoginRepository loginRepository,
            ILogger<LoginService> logger,
            IRefreshTokenRepository refreshTokenRepository,
            IRoleRepository roleRepository,
            ITokenService tokenService
        )
        {
            _apiResultFactory = apiResultFactory;
            _loginRepository = loginRepository;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _roleRepository = roleRepository;
        }
        
        public async Task<ApiResult> LoginUserAsync(string userName, string plainPassword)
        {
            try
            {
                _logger.LogInformation("Attempting login for user {UserName}", userName);

                var existingUser = await _loginRepository.GetUserByNameAsync(userName);

                if (existingUser.Data == null)
                {
                    _logger.LogWarning("Login failed. Username {UserName} not found.", userName);
                    return _apiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, ErrorMessage.USERNAME_NOT_FOUND, ErrorLayer.Service);
                }

                var authenticationStatus = AuthenticationHelper.VerifyLogin(plainPassword, existingUser.Data.HashedPassword);

                if (!authenticationStatus )
                {
                    _logger.LogWarning("\n\nLogin failed for user {UserName}. Invalid credentials.\n\n", userName);
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INVALID_CREDENTIALS, ErrorMessage.INVALID_CREDENTIALS, ErrorLayer.Service);
                }

                var role = await _roleRepository.GetRoleByUserIdAsync(existingUser.Data.UserId);

                var token = _tokenService.GenerateToken(userName, role.Data);
                var refreshToken = _tokenService.GenerateRefreshToken();


                var res = await _refreshTokenRepository.AddRefreshTokenAsyc(existingUser.Data.UserId, refreshToken);

                dynamic result = new
                {
                    token,
                    refreshToken
                };
                _logger.LogInformation("Login successful for user {UserName}", userName);
                return _apiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for user {UserName}.", userName);
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.LOG_IN_ERROR, ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> LogoutUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Attempting logout for user ID {UserId}", userId);

                var result = await _refreshTokenRepository.DeleteRefreshTokenAsyc(userId);
                _logger.LogInformation("Logout successful for user ID {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout for user ID {UserId}.", userId);
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_TOKEN_ERROR, ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> RefreshUserAsync(string refreshToken)
        {
            try
            {
                _logger.LogInformation("Attempting token refresh for refresh token {RefreshToken}", refreshToken);

                var resUserId = await _refreshTokenRepository.GetUserIdByRefreshTokenAsync(refreshToken);
                if (resUserId == null)
                {
                    _logger.LogWarning("Token refresh failed. Refresh token {RefreshToken} is expired or invalid.", refreshToken);
                    return _apiResultFactory.CreateErrorResult(ErrorCode.SESSION_EXPIRED, ErrorMessage.SESSION_EXPIRED, ErrorLayer.Service);
                }

                var user = await _loginRepository.GetUserByIdAsync(resUserId.Data);


                var role = await _roleRepository.GetRoleByUserIdAsync(resUserId.Data);


                var newToken = _tokenService.GenerateToken(user.Data.UserName, role.Data);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                var res = await _refreshTokenRepository.UpdateRefreshTokenAsyc(resUserId.Data, newRefreshToken);

                dynamic result = new
                {
                    newToken,
                    newRefreshToken
                };

                _logger.LogInformation("Token refresh successful for user data: {resUserId}", resUserId);

                return _apiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token refresh for refresh token {RefreshToken}.", refreshToken);
                return _apiResultFactory.CreateErrorResult(ErrorCode.SESSION_EXPIRED, ErrorMessage.SESSION_EXPIRED, ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> RegisterUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Attempting registration for user {UserName}", user.UserName);

                var checkExistingUserName = await _loginRepository.GetUserByNameAsync(user.UserName);


                if (checkExistingUserName.Data != null)
                {
                    _logger.LogWarning("Registration failed. User with username {UserName} already exists.", user.UserName);
                    return _apiResultFactory.CreateErrorResult(ErrorCode.ALREADY_EXIST_ERROR, ErrorMessage.USERNAME_ALREADY_EXIST, ErrorLayer.Service);
                }

                var checkExistingUserEmail = await _loginRepository.GetUserByEmailAsync(user.Email);
                if (checkExistingUserEmail.Data != null)
                {
                    _logger.LogWarning("Registration failed. User with  email {Email} already exists.", user.Email);
                    return _apiResultFactory.CreateErrorResult(ErrorCode.ALREADY_EXIST_ERROR, ErrorMessage.EMAIL_ALREADY_EXIST, ErrorLayer.Service);
                }

                user.HashedPassword = AuthenticationHelper.HashPassword(user.HashedPassword);

                var result = await _loginRepository.AddUserAsync(user);
                _logger.LogInformation("User {UserName} registered successfully.", user.UserName);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for user {UserName}.", user.UserName);
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.REGISTER_USER_ERROR, ErrorLayer.Service);
            }
        }
    }
}
