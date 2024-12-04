using EMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Repositories.Interfaces
{
    public interface IDesignationRepository
    {
        Task<IEnumerable<Designation>> GetAllDesignationsAsync();
        Task<Designation> GetDesignationByIdAsync(int designationId);
        Task<Designation> CreateDesignationAsync(Designation designation);
        Task<Designation> UpdateDesignationAsync(Designation designation);
        Task<bool> DeleteDesignationAsync(int designationId);
    }
}
