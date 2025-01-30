using EMS.DtoMapping.DTOs.EmployeeDTOs;
using EMS.Models;

namespace EMS.Helpers
{
    public static class EmployeeMapper
    {
        public static Employee ToEmployee(this CreateEmployeeDto dto)
        {
            return new Employee
            {
                OfficeEmployeeId = dto.OfficeEmployeeId,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                DOB = dto.DOB,
                DepartmentId = dto.DepartmentId,
                DesignationId = dto.DesignationId
            };
        }

        public static Employee ToEmployee(this UpdateEmployeeDto dto, int employeeId)
        {
            return new Employee
            {
                EmployeeId = employeeId,
                OfficeEmployeeId = dto.OfficeEmployeeId,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                DOB = dto.DOB,
                DepartmentId = dto.DepartmentId,
                DesignationId = dto.DesignationId
            };
        }

    }
}
