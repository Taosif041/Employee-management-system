using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EMS.Helpers.Enums;

namespace EMS.Services.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IOperationLogRepository _operationLogRepository;

        private readonly OperationLogger _operationLogger;
        private readonly ApiResultFactory _apiResultFactory;


        public AttendanceService(IAttendanceRepository attendanceRepository, IOperationLogRepository operationLogRepository, ApiResultFactory apiResultFactory)
        {
            _attendanceRepository = attendanceRepository;
            _operationLogRepository = operationLogRepository;
            _operationLogger = new OperationLogger(_operationLogRepository);
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> GetAllAttendanceAsync()
        {
            try
            {
                var result = await _attendanceRepository.GetAllAttendanceAsync();
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.GET_ATTENDANCE_ERROR);
            }
        }

        public async Task<ApiResult> GetAttendanceByEmployeeIdAndDateAsync(int employeeId, DateTime date)
        {
            try
            {
                var result = await _attendanceRepository.GetAttendanceByIdAndDateAsync(employeeId, date);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.GET_ATTENDANCE_ERROR);
            }
        }

        public async Task<ApiResult> CreateAttendanceAsync(Attendance attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, ErrorMessage.VALIDATION_ERROR);
                }

                var result = await _attendanceRepository.CreateAttendanceAsync(attendance);
                return result;
            }
            
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_ATTENDANCE_ERROR);
            }
        }

        public async Task<ApiResult> UpdateAttendanceAsync(Attendance attendance)
        {
            try
            {
                var result = await _attendanceRepository.UpdateAttendanceAsync(attendance);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_ATTENDANCE_ERROR);
            }
        }

        public async Task<ApiResult> DeleteAttendanceAsync(int employeeId, DateTime date)
        {
            try
            {
                if (employeeId <= 0 || date == default)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, ErrorMessage.VALIDATION_ERROR);
                }

                var deletionSuccess = await _attendanceRepository.DeleteAttendanceAsync(employeeId, date);

                return deletionSuccess;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_ATTENDANCE_ERROR);
            }
        }
    }
}
