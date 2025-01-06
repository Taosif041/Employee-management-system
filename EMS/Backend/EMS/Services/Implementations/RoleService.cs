using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;

namespace EMS.Services.Implementations
{
    public class RoleService: IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly ApiResultFactory _apiResultFactory;

        public RoleService(
            IRoleRepository roleRepository, 
            ILoginRepository loginRepository,
            ApiResultFactory apiResultFactory
            )
        {
            _roleRepository = roleRepository;
            _loginRepository = loginRepository;
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> AssignRoleToUserIdAsync(int userId, int role)
        {
            var user = await _loginRepository.GetUserByIdAsync(userId);

            if (user.Data == null) {
                return _apiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, ErrorMessage.USER_NOT_FOUND, ErrorLayer.Service);
            }

            var result = await _roleRepository.AssignRoleToUserIdAsync(userId, role);
            return result;
        }

        public async Task<ApiResult> GetRoleByUserIdAsync(int userId)
        {
            var user = await _loginRepository.GetUserByIdAsync(userId);
            if (user.Data == null)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, ErrorMessage.USER_NOT_FOUND, ErrorLayer.Service);
            }
            var result = await _roleRepository.GetRoleByUserIdAsync(userId);
            return result;
        }

    }
}
