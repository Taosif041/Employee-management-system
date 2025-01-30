using System.ComponentModel.DataAnnotations;

namespace EMS.DtoMapping.DTOs.EmployeeDTOs
{
    public class CreateEmployeeDto
    {
        [Required]
        public int OfficeEmployeeId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name should be between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public DateTime? DOB { get; set; }

        public int? DepartmentId { get; set; }

        public int? DesignationId { get; set; }
    }
}
