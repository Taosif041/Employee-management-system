using Npgsql;
using EMS.Models;
using EMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using EMS.Helpers;
using static EMS.Data.Enums;
using Npgsql;
using Dapper;


namespace EMS.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly OperationLogger _operationLogger;

        public AttendanceRepository(NpgsqlConnection connection, IOperationLogRepository operationLogRepository)
        {
            _connection = connection;
            _operationLogRepository = operationLogRepository;
            _operationLogger = new OperationLogger(_operationLogRepository);
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendanceAsync()
        {
            try
            {
                await _connection.OpenAsync();

                // Corrected function call
                const string query = "SELECT * FROM get_all_attendances();";
                var attendanceList = await _connection.QueryAsync<Attendance>(query); // or use the query string with a parameterized call

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, null, OperationType.GetAll);

                return attendanceList;
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while fetching attendance records.", ex);
            }

        }

        public async Task<Attendance> GetAttendanceByIdAndDateAsync(int employeeId, string date)
        {
            try
            {
                await _connection.OpenAsync();
                var query = "SELECT * FROM get_attendance_by_id_and_date(@EmployeeId, @Date)";
                Console.WriteLine("hello repo->", date);

                var parameters = new { EmployeeId = employeeId, Date = date.ToString() };

                var attendance = await _connection.QueryFirstOrDefaultAsync<Attendance>(query, parameters);

                Console.WriteLine(attendance);

                await _operationLogger.LogOperationAsync(EntityName.Attendance, employeeId, OperationType.GetById);

                return attendance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: -> repo -> {ex.Message}, {date}");
                throw new Exception("An error occurred while retrieving attendance data.", ex);
            }
        }

        public async Task<Attendance> CreateAttendanceAsync(Attendance attendance)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new
                {
                    attendance.EmployeeId,
                    attendance.Date,
                    attendance.CheckInTime,
                    attendance.CheckOutTime,
                    attendance.Status
                };

                var newId = await _connection.ExecuteScalarAsync<int>("create_attendance", parameters, commandType: CommandType.StoredProcedure);

                attendance.AttendanceId = newId;

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, newId, OperationType.Create);

                return attendance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while creating attendance record.", ex);
            }
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance)
        {
            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance), "Attendance object cannot be null.");

            try
            {
                // Retrieve the current attendance record from the database
                var currentAttendance = await GetAttendanceByIdAndDateAsync(attendance.EmployeeId, attendance.Date.ToString("yyyy-mm-dd"));

                if (currentAttendance == null)
                    throw new Exception($"Attendance record for employee {attendance.EmployeeId} on {attendance.Date} not found.");

                var parameters = new
                {
                    AttendanceId = attendance.AttendanceId,
                    attendance.EmployeeId,
                    attendance.Date,
                    attendance.CheckInTime,
                    attendance.CheckOutTime,
                    attendance.Status
                };

                // Call the stored procedure to update the attendance record
                await _connection.ExecuteAsync("update_attendance", parameters, commandType: CommandType.StoredProcedure);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, attendance.AttendanceId, OperationType.Update);

                return attendance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while updating attendance record.", ex);
            }
        }

        public async Task<bool> DeleteAttendanceAsync(int employeeId, DateTime date)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new { EmployeeId = employeeId, Date = date };

                bool status = await _connection.ExecuteScalarAsync<bool>("delete_attendance", parameters, commandType: CommandType.StoredProcedure);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, employeeId, OperationType.Delete);

                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while deleting attendance record.", ex);
            }
        }
    }
}
