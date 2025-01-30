namespace EMS.DtoMapping.DTOs.Attendance
{
    public class CreateAttendanceDto
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; }
    }

    public class UpdateAttendanceDto
    {
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; }
    }
}
