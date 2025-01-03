﻿namespace EMS.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public int EmployeeId { get; set; }               // Required for update
        public string OfficeEmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
    }

}
