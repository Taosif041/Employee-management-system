using EMS.Models;
using EMS.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IDesignationService
    {
        Task<ApiResult> GetAllDesignationsAsync();
        Task<ApiResult> GetDesignationByIdAsync(int designationId);
        Task<ApiResult> CreateDesignationAsync(CreateDesignationDto dto);
        Task<ApiResult> UpdateDesignationAsync(int designationId, UpdateDesignationDto dto);
        Task<ApiResult> DeleteDesignationAsync(int designationId);
    }
}
