using System.ComponentModel.DataAnnotations;

namespace EMS.DtoMapping.DTOs.EmployeeDTOs
{
    public class DeleteEmployeeDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
