using EMS.Helpers;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EMS.Data.Enums;

namespace EMS.Services.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly OperationLogger _operationLogger;

        public AttendanceService(IAttendanceRepository attendanceRepository, IOperationLogRepository operationLogRepository)
        {
            _attendanceRepository = attendanceRepository;
            _operationLogRepository = operationLogRepository;
            _operationLogger = new OperationLogger(_operationLogRepository);
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendanceAsync()
        {
            try
            {
                var attendanceList = await _attendanceRepository.GetAllAttendanceAsync();

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, null, OperationType.GetAll);

                return attendanceList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while retrieving all attendance records.", ex);
            }
        }

        public async Task<Attendance> GetAttendanceByEmployeeIdAndDateAsync(int employeeId, DateTime date)
        {
            try
            {
                var Dat = date.ToString("yyyy-MM-dd");
                var attendance = await _attendanceRepository.GetAttendanceByIdAndDateAsync(employeeId, Dat);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, employeeId, OperationType.GetById);

                return attendance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: hello darkness -> service {ex.Message}");
                throw new Exception($"An error occurred while retrieving attendance for employee {employeeId} on {date.ToString("yyyy-MM-dd")}.", ex);
            }
        }

        public async Task<Attendance> CreateAttendanceAsync(Attendance attendance)
        {
            try
            {
                if (attendance == null)
                {
                    throw new ArgumentNullException(nameof(attendance), "Attendance cannot be null.");
                }

                var newAttendance = await _attendanceRepository.CreateAttendanceAsync(attendance);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, newAttendance.AttendanceId, OperationType.Create);

                return newAttendance;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Invalid attendance data provided.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while creating the attendance record.", ex);
            }
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance)
        {
            try
            {
                if (attendance == null)
                {
                    throw new ArgumentNullException(nameof(attendance), "Attendance cannot be null.");
                }

                // Check if the attendance exists before attempting an update
                var currentAttendance = await _attendanceRepository.GetAttendanceByIdAndDateAsync(attendance.EmployeeId, attendance.Date.ToString("yyyy-mm-dd"));
                if (currentAttendance == null)
                {
                    throw new Exception($"Attendance record not found for employee {attendance.EmployeeId} on {attendance.Date}");
                }

                var updatedAttendance = await _attendanceRepository.UpdateAttendanceAsync(attendance);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, updatedAttendance.AttendanceId, OperationType.Update);

                return updatedAttendance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while updating the attendance record.", ex);
            }
        }

        public async Task<bool> DeleteAttendanceAsync(int employeeId, DateTime date)
        {
            try
            {
                // Ensure that the employeeId and date are valid
                if (employeeId <= 0 || date == default)
                {
                    throw new ArgumentException("Invalid employeeId or date provided.");
                }

                var deletionSuccess = await _attendanceRepository.DeleteAttendanceAsync(employeeId, date);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, employeeId, OperationType.Delete);

                return deletionSuccess;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Invalid input parameters for deletion.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while deleting the attendance record.", ex);
            }
        }
    }
}
