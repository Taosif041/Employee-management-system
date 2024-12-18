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

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // Get all attendance records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var attendanceRecords = await _attendanceService.GetAllAttendanceAsync();
                if (attendanceRecords == null)
                {
                    return NotFound("No attendance records found. kk");
                }

                return Ok(attendanceRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(50, ex.Message); // Internal server error
            }
        }

        // Get attendance for a specific employee on a specific date
        [HttpGet("{employeeId}/{date}")]
        public async Task<IActionResult> GetAttendanceByEmployeeIdAndDateAsync(int employeeId, DateTime date)
        {
            try
            {
                var attendance = await _attendanceService.GetAttendanceByEmployeeIdAndDateAsync(employeeId, date);
                if (attendance == null)
                {
                    return NotFound("Attendance record not found.");
                }

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        // Create a new attendance record
        [HttpPost]
        public async Task<IActionResult> CreateAttendance([FromBody] Attendance attendance)
        {
            try
            {
                if (attendance == null)
                {
                    return BadRequest("Invalid attendance data.");
                }

                var result = await _attendanceService.CreateAttendanceAsync(attendance);
                return Ok(result); // Return the created attendance record
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Invalid input
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Internal server error
            }
        }

        // Update an existing attendance record
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance([FromBody] Attendance attendance)
        {
            try
            {
                

                var updatedAttendance = await _attendanceService.UpdateAttendanceAsync(attendance);
                if (updatedAttendance == null)
                {
                    return NotFound("Attendance record not found.");
                }

                return Ok(updatedAttendance); // Return updated attendance record
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Invalid attendance data.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Internal server error
            }
        }

        // Delete a specific attendance record
        [HttpDelete("{employeeId}/{date}")]
        public async Task<IActionResult> DeleteAttendance(int employeeId, DateTime date)
        {
            try
            {
                var success = await _attendanceService.DeleteAttendanceAsync(employeeId, date);
                if (success)
                {
                    return Ok(); // Successfully deleted
                }

                return NotFound("Attendance record not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Invalid input
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Internal server error
            }
        }
    }
}
