using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EMS.Helpers.Enums;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationLogController : ControllerBase
    {
        private readonly IOperationLogService _operationLogService;
        private readonly ApiResultFactory _apiResultFactory;


        public OperationLogController(IOperationLogService operationLogService, ApiResultFactory apiResultFactory)
        {
            _operationLogService = operationLogService;
            _apiResultFactory = apiResultFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogsAsync()
        {
            try
            {
                var result = await _operationLogService.GetAllLogsAsync();

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_LOG_ERROR));
            }
        }

        //[HttpGet("search")]
        //public async Task<IActionResult> GetLogsWithParametersAsync([FromQuery] string? operationType = null,
        //                                                           [FromQuery] string? entityName = null,
        //                                                           [FromQuery] int? entityId = null)
        //{
        //    try
        //    {
        //        var result = await _operationLogService.GetLogsWithParametersAsync(operationType, entityName, entityId);

        //        if (result.IsSuccess) return Ok(result);

        //        return StatusCode((int)result.ErrorCode, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_LOG_ERROR));
        //    }
        //}
    }
}
