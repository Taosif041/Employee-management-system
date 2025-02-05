﻿using EMS.DtoMapping.DTOs.DepartmentDTOs;
using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Models.DTOs;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ApiResultFactory _apiResultFactory;
        
        public DepartmentController(IDepartmentService departmentService, ApiResultFactory apiResultFactory)
        {
            _departmentService = departmentService;
            _apiResultFactory = apiResultFactory;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetAllDepartmentAsync()
        {
            try
            {
                var result = await _departmentService.GetAllDepartmentsAsync();

                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult((int)result.ErrorCode, result.ErrorMessage, result.ErrorLayer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));
            }
        }


        [HttpGet("{departmentId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                var result = await _departmentService.GetDepartmentByIdAsync(departmentId);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult((int)result.ErrorCode, result.ErrorMessage, result.ErrorLayer));

            }
            catch (Exception ex)
            {
               return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));

            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentDto dto)
        {
            try
            {
                var result = await _departmentService.CreateDepartmentAsync(dto);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult((int)result.ErrorCode, result.ErrorMessage, result.ErrorLayer));
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_DEPARTMENT_ERROR));
            }
        }

        [HttpPut("{departmentId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateDepartmentAsync(int departmentId, UpdateDepartmentDto dto)
        {
            try
            {

                var result = await _departmentService.UpdateDepartmentAsync(departmentId, dto);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult((int)result.ErrorCode, result.ErrorMessage, result.ErrorLayer));
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DEPARTMENT_ERROR));
            }
        }

        [HttpDelete("{departmentId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            try
            {
                var result = await _departmentService.DeleteDepartmentAsync(departmentId);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult((int)result.ErrorCode, result.ErrorMessage, result.ErrorLayer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DEPARTMENT_ERROR));
            }
        }
    }
}
