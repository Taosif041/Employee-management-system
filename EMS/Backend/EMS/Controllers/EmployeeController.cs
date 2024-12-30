using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ClosedXML.Excel;


namespace EMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController: ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ApiResultFactory _apiResultFactory;
        private readonly IConverterService _converterService;
        public EmployeeController(IEmployeeService employeeService, ApiResultFactory apiResultFactory, IConverterService converterService)
        {
            _employeeService = employeeService;
            _apiResultFactory = apiResultFactory;
            _converterService = converterService;
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
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR, ErrorLayer.Controller));
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
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_EMPLOYEE_ERROR, ErrorLayer.Controller));
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
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_EMPLOYEE_ERROR, ErrorLayer.Controller));
            }
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, Employee employee)
        {
            try
            {
                if (employeeId != employee.EmployeeId)
                {
                    return StatusCode(400, _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, ErrorMessage.UPDATE_EMPLOYEE_ERROR, ErrorLayer.Controller));
                }

                var result = await _employeeService.UpdateEmployeeInformationAsync(employee);

                if (result.IsSuccess)
                    return Ok(result.Data);

                return StatusCode((int)result.ErrorCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_EMPLOYEE_ERROR, ErrorLayer.Controller));
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
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_EMPLOYEE_ERROR, ErrorLayer.Controller));
            }
        }

        [HttpGet("download-csv")]
        public async Task<IActionResult> GetEmGetEmployeeCSVAsync()
        {
            try
            {
                var result = await _converterService.GetEmployeeCSVAsync();
                if (result.IsSuccess)
                {
                    return File(result.Data, "text/csv", "EmployeeList.csv");
                }
                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Employee CSV creation Error", ErrorLayer.Controller));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Employee CSV creation Error", ErrorLayer.Controller));
            }
        }

        [HttpGet("download-xlsx")]
        public async Task<IActionResult> GetEmployeeExcelAsync()
        {
            try
            {
                var result = await _converterService.GetEmployeeExcelAsync();
                if (result.IsSuccess)
                {
                    return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeList.xlsx");

                }
                return StatusCode((int)result.ErrorCode, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Employee Excel creation Error", ErrorLayer.Controller));
            }
            catch (Exception ex)
            {
                return StatusCode(500, _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, "Employee Excel creation Error", ErrorLayer.Controller));
            }
        }








    }
}
