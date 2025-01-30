using EMS.Models;
using EMS.Models.DTOs;

namespace EMS.Helpers.Mappers
{
    public static class DesignationMapper
    {
        public static Designation ToDesignation(this CreateDesignationDto dto)
        {
            return new Designation
            {
                Name = dto.Name
            };
        }

        public static Designation ToDesignation(this UpdateDesignationDto dto, int designationId)
        {
            return new Designation
            {
                Name = dto.Name
            };
        }
    }
}
