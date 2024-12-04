namespace EMS.DTOs.Employee
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string OfficeEmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public string DepartmentName { get; set; }        // Populated from Department table
        public string DesignationName { get; set; }       // Populated from Designation table
    }

}
