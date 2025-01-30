using EMS.DtoMapping.DTOs.DepartmentDTOs;
using EMS.Models;

namespace EMS.DtoMapping.Mappers
{
    public static class DepartmentMapper
    {
        public static Department ToDepartment(this CreateDepartmentDto dto)
        {
            return new Department
            {
                Name = dto.Name
            };
        }

        public static Department ToDepartment(this UpdateDepartmentDto dto, int departmentId)
        {
            return new Department
            {
                DepartmentId = departmentId,
                Name = dto.Name
            };
        }
    }

}
