using EMS.DtoMapping.DTOs.Attendance;
using EMS.DtoMapping.Mappers;
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

        public async Task<ApiResult> GetAttendanceByAttendanceId(int attendanceId)
        {
            try
            {
                var result = await _attendanceRepository.GetAttendanceByAttendanceId(attendanceId);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.GET_ATTENDANCE_ERROR);
            }
        }

        public async Task<ApiResult> CreateAttendanceAsync(CreateAttendanceDto dto)
        {
            Attendance attendance = dto.ToAttendance();
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

        public async Task<ApiResult> UpdateAttendanceAsync(int attendanceId, UpdateAttendanceDto dto)
        {
            
            try
            {
                var existingAttendance = await _attendanceRepository.GetAttendanceByAttendanceId(attendanceId);
                if (existingAttendance.Data == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, 
                        "Attendance record not found.", ErrorLayer.Service);
                }
                Attendance existingAttendanceEntity = existingAttendance.Data;

                Attendance attendance = dto.ToAttendance(existingAttendanceEntity);
                var result = await _attendanceRepository.UpdateAttendanceAsync(attendance);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_ATTENDANCE_ERROR);
            }
        }

        public async Task<ApiResult> DeleteAttendanceAsync(int attendanceId)
        {
            try
            {
                if (attendanceId == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, ErrorMessage.VALIDATION_ERROR);
                }

                var deletionSuccess = await _attendanceRepository.DeleteAttendanceAsync(attendanceId);

                return deletionSuccess;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_ATTENDANCE_ERROR);
            }
        }

        
    }
}
