using EMS.DtoMapping.DTOs.Attendance;
using EMS.Models;

namespace EMS.DtoMapping.Mappers
{
    public static class AttendanceMapper
    {
        public static Attendance ToAttendance(this CreateAttendanceDto dto)
        {
            return new Attendance
            {
                EmployeeId = dto.EmployeeId,
                Date = dto.Date,
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,
                Status = dto.Status
            };
        }
        public static Attendance ToAttendance(this UpdateAttendanceDto dto, Attendance existingAttendance)
        {
            return new Attendance
            {
                AttendanceId = existingAttendance.AttendanceId, 
                EmployeeId = existingAttendance.EmployeeId,     
                Date = existingAttendance.Date,         
                CheckInTime = dto.CheckInTime ?? existingAttendance.CheckInTime,
                CheckOutTime = dto.CheckOutTime ?? existingAttendance.CheckOutTime,
                Status = dto.Status ?? existingAttendance.Status
            };
        }



    }
}
