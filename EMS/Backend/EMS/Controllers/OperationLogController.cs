using EMS.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationLogController : ControllerBase
    {
        private readonly IOperationLogService _operationLogService;

        public OperationLogController(IOperationLogService operationLogService)
        {
            _operationLogService = operationLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogsAsync()
        {
            try
            {
                var logs = await _operationLogService.GetAllLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllLogs: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the logs.");
            }
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployeeLogsAsync()
        {
            try
            {
                var logs = await _operationLogService.GetEmployeeLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeeLogs: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the employee logs.");
            }
        }

        [HttpGet("employees/{employeeId}")]
        public async Task<IActionResult> GetLogsByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var logs = await _operationLogService.GetLogsByEmployeeIdAsync(employeeId);
                if (logs == null)
                {
                    return NotFound($"No logs found for employee with ID {employeeId}.");
                }

                return Ok(logs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLogsByEmployeeId for employeeId {employeeId}: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the logs.");
            }
        }
    }
}
