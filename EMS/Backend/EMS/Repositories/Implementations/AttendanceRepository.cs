using Npgsql;
using EMS.Models;
using EMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using EMS.Helpers;
using static EMS.Helpers.Enums;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MongoDB.Driver.Linq;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.EMS.Repositories.DatabaseProviders.Implementations;
using EMS.Core.Helpers;


namespace EMS.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly OperationLogger _operationLogger;

        private readonly IDatabaseFactory _databaseFactory;
        private readonly ApiResultFactory _apiResultFactory;

        public AttendanceRepository(
            IOperationLogRepository operationLogRepository,
            IDatabaseFactory databaseFactory,
            ApiResultFactory apiResultFactory)
        {
            _operationLogRepository = operationLogRepository;
            _operationLogger = new OperationLogger(_operationLogRepository);

            _databaseFactory = databaseFactory;
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> GetAllAttendanceAsync()
        {
            using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
            {
                try
                {
                    const string query = "SELECT * FROM get_all_attendances();";
                    var result = await connection.QueryAsync<Attendance>(query);

                    await _operationLogger.LogOperationAsync(EntityName.Attendance, null, OperationType.GetAll);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR);
                }
            }

        }

        public async Task<ApiResult> GetAttendanceByIdAndDateAsync(int employeeId, DateTime date)
        {
            using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
            {
                try
                {
                    string formattedDate = date.ToString("yyyy-MM-dd");



                    var query = "SELECT * FROM get_attendance_by_id_and_date(@EmployeeId, @Date::DATE)";
                    var parameters = new { EmployeeId = employeeId, Date = formattedDate };

                    var result = await connection.QueryFirstOrDefaultAsync<Attendance>(query, parameters);

                    await _operationLogger.LogOperationAsync(EntityName.Attendance, employeeId, OperationType.GetById);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR);
                }
            }
        }


        public async Task<ApiResult> CreateAttendanceAsync(Attendance attendance)
        {
            using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
            {

                try
                {
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

                    var result = await connection.QuerySingleOrDefaultAsync<Attendance>(query, parameters);


                    await _operationLogger.LogOperationAsync(EntityName.Attendance, result.EmployeeId, OperationType.Create);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_ATTENDANCE_ERROR);
                }
            }
        }

        public async Task<ApiResult> UpdateAttendanceAsync(Attendance attendance)
        {
            using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
            {

                try
                {
                    var currentAttendance = await GetAttendanceByIdAndDateAsync(attendance.EmployeeId, attendance.Date);

                    var parameters = new
                    {
                        AttendanceId = attendance.AttendanceId,
                        attendance.EmployeeId,
                        Date = attendance.Date.ToString("yyyy-MM-dd"),
                        CheckInTime = attendance.CheckInTime,
                        CheckOutTime = attendance.CheckOutTime,
                        attendance.Status
                    };

                    var query = "SELECT * FROM update_attendance(@AttendanceId, @EmployeeId, @Date::DATE, @CheckInTime::TIMESTAMP, @CheckOutTime::TIMESTAMP, @Status)";

                    var result = await connection.QuerySingleOrDefaultAsync<Attendance>(query, parameters);



                    await _operationLogger.LogOperationAsync(EntityName.Attendance, result.AttendanceId, OperationType.Update);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_ATTENDANCE_ERROR);
                }
            }
        }


        public async Task<ApiResult> DeleteAttendanceAsync(int employeeId, DateTime date)
        {
            using (IDbConnection connection = _databaseFactory.CreatePostgresSqlConnection())
            {
                try
                {
                    string formattedDate = date.ToString("yyyy-MM-dd");

                    var query = "SELECT * FROM get_attendance_by_id_and_date(@EmployeeId, @Date::DATE)";
                    var parameters = new { EmployeeId = employeeId, Date = formattedDate };

                    var result = await connection.ExecuteScalarAsync<bool>(query, parameters);

                    await _operationLogger.LogOperationAsync(EntityName.Attendance, employeeId, OperationType.Delete);

                    return _apiResultFactory.CreateSuccessResult(result);
                }
                catch (Exception ex)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_ATTENDANCE_ERROR);
                }
            }
        }
    }
}
