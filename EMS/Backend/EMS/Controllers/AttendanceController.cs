using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Text;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using EMS.DtoMapping.DTOs.Attendance;


namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ApiResultFactory _apiResultFactory;
        private readonly ILogger<AttendanceController> _logger;
        private readonly IConverterService _converterService;


        public AttendanceController(
            IAttendanceService attendanceService,
            ApiResultFactory apiResultFactory,
            ILogger<AttendanceController> logger,
            IConverterService converterService)
        {
            _attendanceService = attendanceService;
            _apiResultFactory = apiResultFactory;
            _logger = logger;
            _converterService = converterService;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _attendanceService.GetAllAttendanceAsync();

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR));

            }
            catch (Exception )
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR));
            }
        }

        [HttpGet("{attendanceId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAttendanceByAttendanceId(int attendanceId)
        {
            try
            {
                var result = await _attendanceService.GetAttendanceByAttendanceId(attendanceId);

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_ATTENDANCE_ERROR));
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateAttendance([FromBody] CreateAttendanceDto dto)
        {
            try
            {

                var result = await _attendanceService.CreateAttendanceAsync(dto);

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_ATTENDANCE_ERROR));
            }
        }

        [HttpPut("{attendanceId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateAttendance(int attendanceId, [FromBody] UpdateAttendanceDto dto)
        {
            try
            {

                var result = await _attendanceService.UpdateAttendanceAsync(attendanceId,dto);

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_ATTENDANCE_ERROR));
            }
        }

        [HttpDelete("attendanceId")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteAttendance(int attendanceId)
        {
            try
            {
                var result = await _attendanceService.DeleteAttendanceAsync(attendanceId);

                if (result.IsSuccess) return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_ATTENDANCE_ERROR));
            }
        }
        [HttpGet("download-csv")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAttendanceCSVAsync()
        {
            try
            {
                var result = await _converterService.GetAttendanceCSVAsync();


                if (result.IsSuccess)
                {
                    return File(result.Data, "text/csv", "AttendanceList.csv");
                }

                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Attendance CSV creation Error", ErrorLayer.Controller));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating the attendance CSV.");

                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Error generating the attendance CSV."));
            }

        }

        [HttpGet("download-xlsx")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAttendanceExcelAsync()
        {
            try
            {
                var result = await _converterService.GetAttendanceExcelAsync();
                if (result.IsSuccess)
                {
                    return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AttendanceList.xlsx");

                }
                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Attendance Excel creation Error", ErrorLayer.Controller));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Attendance Excel creation Error", ErrorLayer.Controller));
            }
        }

    }






}
