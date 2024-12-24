using EMS.Models;

namespace EMS.Helpers
{
    public static class EmployeeMapper
    {
        public static EmployeeDTO ToDTO(Employee employee)
        {
            return new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                Address = employee.Address,
                DOB = employee.DOB
            };
        }

        public static List<EmployeeDTO> ToDTOList(List<Employee> employees)
        {
            return employees.Select(e => ToDTO(e)).ToList();
        }
    }
}
