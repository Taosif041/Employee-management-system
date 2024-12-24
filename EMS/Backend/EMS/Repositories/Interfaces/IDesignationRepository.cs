using EMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Repositories.Interfaces
{
    public interface IDesignationRepository
    {
        Task<ApiResult> GetAllDesignationsAsync();
        Task<ApiResult> GetDesignationByIdAsync(int designationId);
        Task<ApiResult> CreateDesignationAsync(Designation designation);
        Task<ApiResult> UpdateDesignationAsync(Designation designation);
        Task<ApiResult> DeleteDesignationAsync(int designationId);
    }
}
