namespace EMS.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }               // Primary Key
        public int OfficeEmployeeId { get; set; }      // User-defined unique ID
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }                 // Date of Birth
        public int? DepartmentId { get; set; }             // Foreign Key to Department
        public int? DesignationId { get; set; }            // Foreign Key to Designation
    }

}
