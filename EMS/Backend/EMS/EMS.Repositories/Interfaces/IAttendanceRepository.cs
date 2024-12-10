using EMS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAttendanceAsync();
        Task<Attendance> GetAttendanceByIdAndDateAsync(int employeeId, string date);
        Task<Attendance> CreateAttendanceAsync(Attendance attendance);
        Task<Attendance> UpdateAttendanceAsync(Attendance attendance);
        Task<bool> DeleteAttendanceAsync(int employeeId, DateTime date);
    }
}
