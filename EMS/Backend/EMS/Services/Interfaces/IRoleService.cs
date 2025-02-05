﻿using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResult> AssignRoleToUserIdAsync(int userId, int role);
        Task<ApiResult> GetRoleByUserIdAsync(int userId);
    }

}
