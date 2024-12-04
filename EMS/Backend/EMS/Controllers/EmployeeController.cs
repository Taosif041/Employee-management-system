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
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            if (employees == null)
                return NotFound();

            return Ok(employees);
        }
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
                return NotFound();

            return Ok(employee);

        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            try
            {
                var result = await _employeeService.CreateEmployeeAsync(employee);
                return Ok(result); // If everything goes well
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Handle null or invalid employee error
            }
            catch (Exception ex)
            {
                // General exception handling (e.g., database issues, etc.)
                return StatusCode(500, ex.Message); // Internal Server Error
            }
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, Employee employee)
        {
            try
            {
                if (employeeId != employee.EmployeeId)
                {
                    return BadRequest("Employee ID mismatch.");
                }

                var updatedEmployee = await _employeeService.UpdateEmployeeInformationAsync(employee);
                if (updatedEmployee == null)
                {
                    return NotFound();
                }


                return Ok(updatedEmployee); // Success
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Invalid employee input."); // Handle invalid employee input
            }
            catch (Exception ex)
            {
                Console.WriteLine("joy bangla");
                return StatusCode(500, ex.Message); // Handle other internal server errors
            }
        }


        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                var success = await _employeeService.DeleteEmployeeAsync(employeeId);
                
                if (success)
                {
                    return Ok(); // Success
                }
                return NotFound("Employee not found"); // Handle employee not found case
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Handle invalid employee ID
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Handle other internal server errors
            }
        }

    }
}
