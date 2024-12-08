using EMS.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EMS.Data.Enums;

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

        // Endpoint to get all logs
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

        // Endpoint to get logs based on parameters (operationType, entityName, entityId)
        [HttpGet("search")]
        public async Task<IActionResult> GetLogsWithParametersAsync([FromQuery] string operationType = null,
                                                                   [FromQuery] string entityName = null,
                                                                   [FromQuery] int? entityId = null)
        {
            try
            {
                var logs = await _operationLogService.GetLogsWithParametersAsync(
                    operationType != null ? Enum.TryParse(operationType, out OperationType opType) ? opType : (OperationType?)null : null,
                    entityName != null ? Enum.TryParse(entityName, out EntityName entName) ? entName : (EntityName?)null : null,
                    entityId);

                return Ok(logs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLogsWithParameters: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the logs with parameters.");
            }
        }
    }
}
