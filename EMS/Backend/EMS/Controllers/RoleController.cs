using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ApiResultFactory _apiResultFactory;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ApiResultFactory apiResultFactory, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _apiResultFactory = apiResultFactory;
            _logger = logger;  
        }

        [HttpPost("assign")]
        [Authorize(Policy = "SuperAdminPolicy")]
        public async Task<IActionResult> AssignRole(int userId, int role)
        {
            try
            {
                var result = await _roleService.AssignRoleToUserIdAsync(userId, role);

                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }

                _logger.LogError($"Failed to assign role {role} to user {userId}: {result.ErrorMessage}");

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
                _logger.LogError($"Error assigning role to user {userId}: {ex.Message}", ex);

                return StatusCode(
                    500,
                    _apiResultFactory.CreateErrorResult(
                        ErrorCode.INTERNAL_SERVER_ERROR,
                        ErrorMessage.AUTHORIZATION_ERROR,
                        ErrorLayer.Controller
                    )
                );
            }
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetRole(int userId)
        {
            try
            {
                // Get role
                var result = await _roleService.GetRoleByUserIdAsync(userId);

                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }

                _logger.LogError($"Failed to get role for user {userId}: {result.ErrorMessage}");

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
                _logger.LogError($"Error fetching role for user {userId}: {ex.Message}", ex);

                return StatusCode(
                    500,
                    _apiResultFactory.CreateErrorResult(
                        ErrorCode.INTERNAL_SERVER_ERROR,
                        ErrorMessage.AUTHORIZATION_ERROR,
                        ErrorLayer.Controller
                    )
                );
            }
        }
    }
}
