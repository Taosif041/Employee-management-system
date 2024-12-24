using EMS.Core.Helpers;
using EMS.Helpers;
using EMS.Models;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController: ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ApiResultFactory _apiResultFactory;
        public EmployeeController(IEmployeeService employeeService, ApiResultFactory apiResultFactory)
        {
            _employeeService = employeeService;
            _apiResultFactory = apiResultFactory;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _employeeService.GetAllEmployeesAsync();

                if (result.IsSuccess)return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

                if (result.IsSuccess)
                    return Ok(result.Data);
                return StatusCode((int)result.ErrorCode, result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            try
            {
                var result = await _employeeService.CreateEmployeeAsync(employee);
                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_EMPLOYEE_ERROR));
            }
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, Employee employee)
        {
            try
            {
                if (employeeId != employee.EmployeeId)
                {
                    return StatusCode(400, _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, ErrorMessage.UPDATE_EMPLOYEE_ERROR));
                }

                var result = await _employeeService.UpdateEmployeeInformationAsync(employee);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_EMPLOYEE_ERROR));
            }
        }


        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(employeeId);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
           
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_EMPLOYEE_ERROR));
            }
        }

    }
}
