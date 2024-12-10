using EMS.Models;

namespace EMS.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAllAttendanceAsync();  // Fetches all attendance records
        Task<Attendance> GetAttendanceByEmployeeIdAndDateAsync(int employeeId, DateTime date);  // Fetches attendance for a specific employee on a specific date
        Task<Attendance> CreateAttendanceAsync(Attendance attendance);  // Creates a new attendance record
        Task<Attendance> UpdateAttendanceAsync(Attendance attendance);  // Updates an existing attendance record
        Task<bool> DeleteAttendanceAsync(int employeeId, DateTime date);  // Deletes a specific attendance record by employee ID and date
    }
}
