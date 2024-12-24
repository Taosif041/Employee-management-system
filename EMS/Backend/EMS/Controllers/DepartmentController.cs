using EMS.Core.Helpers;
using EMS.Helpers;
using EMS.Models;
using EMS.Services.Interfaces;
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
        public async Task<IActionResult> GetAllDepartmentAsync()
        {
            try
            {
                var result = await _departmentService.GetAllDepartmentsAsync();

                if (result.IsSuccess)
                    return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));
            }
        }


        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                var result = await _departmentService.GetDepartmentByIdAsync(departmentId);

                if (result.IsSuccess)
                    return Ok(result);

                return StatusCode((int)result.ErrorCode, result);

            }
            catch (Exception ex)
            {
               return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.
                    INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));

            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartmentAsync(Department department)
        {
            try
            {
                var result = await _departmentService.CreateDepartmentAsync(department);

                if (result.IsSuccess)
                    return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_DEPARTMENT_ERROR));
            }
        }

        [HttpPut("{departmentId}")]
        public async Task<IActionResult> UpdateDepartmentAsync(int departmentId, Department department)
        {
            try
            {
                if (departmentId != department.DepartmentId)
                {
                    return BadRequest("Department ID mismatch.");
                }

                var result = await _departmentService.UpdateDepartmentAsync(department);

                if (result.IsSuccess)
                    return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DEPARTMENT_ERROR));
            }
        }

        [HttpDelete("{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            try
            {
                var result = await _departmentService.DeleteDepartmentAsync(departmentId);

                if (result.IsSuccess)
                    return Ok(result);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DEPARTMENT_ERROR));
            }
        }
    }
}
