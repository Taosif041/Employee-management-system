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
using static System.Runtime.InteropServices.JavaScript.JSType;
using MongoDB.Driver.Linq;


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

        public async Task<Attendance> GetAttendanceByIdAndDateAsync(int employeeId, DateTime date)
        {
            try
            {
                string formattedDate = date.ToString("yyyy-MM-dd");

                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }

                var query = "SELECT * FROM get_attendance_by_id_and_date(@EmployeeId, @Date::DATE)";
                var parameters = new { EmployeeId = employeeId, Date = formattedDate };

                // Execute the query and retrieve attendance
                var attendance = await _connection.QueryFirstOrDefaultAsync<Attendance>(query, parameters);
                Console.WriteLine(attendance);

                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, employeeId, OperationType.GetById);

                return attendance ?? throw new Exception("No attendance record found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: -> repo ->{ex.Message}, Date: {date:yyyy-MM-dd}");
                throw new Exception("An error occurred while retrieving attendance data.", ex);
            }
        }


        public async Task<Attendance> CreateAttendanceAsync(Attendance attendance)
        {
            try
            {
                await _connection.OpenAsync();
                string date = attendance.Date.ToString("yyyy-MM-dd");
                var parameters = new
                {
                    attendance.EmployeeId,
                    date,
                    attendance.CheckInTime,
                    attendance.CheckOutTime,
                    attendance.Status
                };
                var query = "SELECT * FROM create_attendance(@EmployeeId, @Date::DATE, @CheckInTime, @CheckOutTime, @Status)";

                var createdAttendance = await _connection.QuerySingleOrDefaultAsync<Attendance>(query, parameters);


                // Log the operation
                await _operationLogger.LogOperationAsync(EntityName.Attendance, createdAttendance.EmployeeId, OperationType.Create);

                return createdAttendance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error-repo: {ex.Message}");
                throw new Exception("--repo -- > An error occurred while creating attendance record.", ex);
            }
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance)
        {
            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance), "Attendance object cannot be null.");

            try
            {
                // Retrieve the current attendance record from the database
                var currentAttendance = await GetAttendanceByIdAndDateAsync(attendance.EmployeeId, attendance.Date);

                if (currentAttendance == null)
                    throw new Exception($"Attendance record for employee {attendance.EmployeeId} on {attendance.Date} not found.");

                // Define the parameters for the stored procedure
                var parameters = new
                {
                    AttendanceId = attendance.AttendanceId,
                    attendance.EmployeeId,
                    Date = attendance.Date.ToString("yyyy-MM-dd"), // Ensure date is correctly formatted
                    CheckInTime = attendance.CheckInTime, // Ensure timestamp is correctly formatted
                    CheckOutTime = attendance.CheckOutTime, // Ensure timestamp is correctly formatted
                    attendance.Status
                };

                // Call the stored procedure to update the attendance record
                var query = "SELECT * FROM update_attendance(@AttendanceId, @EmployeeId, @Date::DATE, @CheckInTime::TIMESTAMP, @CheckOutTime::TIMESTAMP, @Status)";

                // Execute the stored procedure and fetch the updated attendance
                var updatedAttendance = await _connection.QuerySingleOrDefaultAsync<Attendance>(query, parameters);

                if (updatedAttendance == null)
                {
                    throw new Exception("Failed to update the attendance record.");
                }

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
                await _connection.OpenAsync();
                string formattedDate = date.ToString("yyyy-MM-dd");

                var query = "SELECT * FROM get_attendance_by_id_and_date(@EmployeeId, @Date::DATE)";
                var parameters = new { EmployeeId = employeeId, Date = formattedDate };

                var status = await _connection.ExecuteScalarAsync<bool>(query, parameters);

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
