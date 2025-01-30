using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Models.DTOs;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;
        private readonly ApiResultFactory _apiResultFactory;

        public DesignationController(IDesignationService designationService, ApiResultFactory apiResultFactory)
        {
            _designationService = designationService;
            _apiResultFactory = apiResultFactory;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAllDesignationAsync()
        {
            try
            {
                var result = await _designationService.GetAllDesignationsAsync();

                if (result.IsSuccess)return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR));
            }
        }

        [HttpGet("{designationId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetDesignationByIdAsync(int designationId)
        {
            try
            {
                var result = await _designationService.GetDesignationByIdAsync(designationId);

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR));
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateDesignationAsync([FromBody] CreateDesignationDto dto)
        {
            try
            {
                var result = await _designationService.CreateDesignationAsync(dto);

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);

            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_DESIGNATION_ERROR));
            }
        }

        [HttpPut("{designationId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateDesignationAsync(int designationId, [FromBody] UpdateDesignationDto dto)
        {
            try
            {
                var result = await _designationService.UpdateDesignationAsync(designationId, dto);

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DESIGNATION_ERROR));
            }
        }

        [HttpDelete("{designationId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteDesignationAsync(int designationId)
        {
            try
            {
                var result = await _designationService.DeleteDesignationAsync(designationId);

                if (result.IsSuccess) return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DESIGNATION_ERROR));
            }
        }
    }
}
