using EMS.Core.Helpers;
using EMS.Helpers;
using EMS.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ApiResultFactory _apiResultFactory;

        public AttendanceController(IAttendanceService attendanceService, ApiResultFactory apiResultFactory)
        {
            _attendanceService = attendanceService;
            _apiResultFactory = apiResultFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _attendanceService.GetAllAttendanceAsync();

                if (result.IsSuccess) return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR));
            }
        }

        [HttpGet("{employeeId}/{date}")]
        public async Task<IActionResult> GetAttendanceByEmployeeIdAndDateAsync(int employeeId, DateTime date)
        {
            try
            {
                var result = await _attendanceService.GetAttendanceByEmployeeIdAndDateAsync(employeeId, date);

                if (result.IsSuccess) return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendance([FromBody] Attendance attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, ErrorMessage.CREATE_ATTENDANCE_ERROR));
                }

                var result = await _attendanceService.CreateAttendanceAsync(attendance);

                if (result.IsSuccess) return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_ATTENDANCE_ERROR));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance([FromBody] Attendance attendance)
        {
            try
            {

                var result = await _attendanceService.UpdateAttendanceAsync(attendance);

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_ATTENDANCE_ERROR));
            }
        }

        [HttpDelete("{employeeId}/{date}")]
        public async Task<IActionResult> DeleteAttendance(int employeeId, DateTime date)
        {
            try
            {
                var result = await _attendanceService.DeleteAttendanceAsync(employeeId, date);

                if (result.IsSuccess) return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_ATTENDANCE_ERROR));
            }
        }
    }
}
