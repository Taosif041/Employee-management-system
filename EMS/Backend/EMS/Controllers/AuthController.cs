using EMS.DTOs.Authentication;
using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ApiResultFactory _apiResultFactory;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILoginService loginService, ApiResultFactory apiResultFactory, ILogger<AuthController> logger)
        {
            _loginService = loginService;
            _apiResultFactory = apiResultFactory;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var result = await _loginService.RegisterUserAsync(user);

                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }

                _logger.LogError(
                    "Registration failed for user {UserName}. Error: {ErrorCode}, ErrorLayer: {ErrorLayer}",
                    user.UserName,
                    result.ErrorCode,
                    result.ErrorLayer
                );

                return StatusCode(
                    (int)result.ErrorCode,
                    _apiResultFactory.CreateErrorResult(
                        (int)result.ErrorCode,
                        result.ErrorMessage,
                        result.ErrorLayer
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while registering user {UserName}. ErrorLayer: {ErrorLayer}",
                    user.UserName,
                    ErrorLayer.Controller
                );

                return StatusCode(
                    500,
                    _apiResultFactory.CreateErrorResult(
                        ErrorCode.INTERNAL_SERVER_ERROR,
                        ErrorMessage.REGISTER_USER_ERROR,
                        ErrorLayer.Controller
                    )
                );
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                var result = await _loginService.LoginUserAsync(login.Username, login.Password);

                if (result.IsSuccess)
                    return Ok(result.Data);

               
                _logger.LogError("Login failed for user {Username}. " +
                    "Error: {ErrorCode}, ErrorLayer: {ErrorLayer}", 
                    login.Username, result.ErrorCode, result.ErrorLayer);

                return StatusCode(
                    (int)result.ErrorCode,
                    _apiResultFactory.CreateErrorResult(
                        (int)result.ErrorCode,
                        result.ErrorMessage,
                        result.ErrorLayer
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user {Username}. ErrorLayer: {ErrorLayer}", login.Username, ErrorLayer.Controller);

                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.LOG_IN_ERROR, ErrorLayer.Controller));
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            try
            {
                var result = await _loginService.RefreshUserAsync(refreshToken);

                if (result.IsSuccess)
                    return Ok(result.Data);

                _logger.LogError("Refresh failed for token {RefreshToken}. Error: {ErrorCode}, ErrorLayer: {ErrorLayer}", refreshToken, result.ErrorCode, result.ErrorLayer);

                return StatusCode(
                    (int)result.ErrorCode,
                    _apiResultFactory.CreateErrorResult(
                        (int)result.ErrorCode,
                        result.ErrorMessage,
                        result.ErrorLayer
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while refreshing token {RefreshToken}. ErrorLayer: {ErrorLayer}", refreshToken, ErrorLayer.Controller);
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.SESSION_EXPIRED, ErrorMessage.SESSION_EXPIRED, ErrorLayer.Controller));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody] int userId)
        {
            try
            {
                var result = await _loginService.LogoutUserAsync(userId);

                if (result.IsSuccess)
                    return Ok(result.Data);

                _logger.LogError("Logout failed for user ID {UserId}. Error: {ErrorCode}, ErrorLayer: {ErrorLayer}", userId, result.ErrorCode, result.ErrorLayer);

                return StatusCode(
                    (int)result.ErrorCode,
                    _apiResultFactory.CreateErrorResult(
                        (int)result.ErrorCode,
                        result.ErrorMessage,
                        result.ErrorLayer
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging out user ID {UserId}. ErrorLayer: {ErrorLayer}", userId, ErrorLayer.Controller);
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.SESSION_EXPIRED, ErrorMessage.SESSION_EXPIRED, ErrorLayer.Controller));
            }
        }
    }
}
