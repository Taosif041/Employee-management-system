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

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            if (departments == null)
                return NotFound();

            return Ok(departments);
        }




        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetDepartmentByIdAsync(int departmentId)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(departmentId);
            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartmentAsync(Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest("Department cannot be null.");
                }

                var result = await _departmentService.CreateDepartmentAsync(department);
                return Ok(result); // If everything goes well
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Handle null or invalid department error
            }
            catch (Exception ex)
            {
                // General exception handling (e.g., database issues, etc.)
                return StatusCode(500, ex.Message); // Internal Server Error
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

                var updatedDepartment = await _departmentService.UpdateDepartmentAsync(department);
                if (updatedDepartment == null)
                {
                    return NotFound();
                }

                return Ok(updatedDepartment); // Success
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Invalid department input."); // Handle invalid department input
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating department");
                return StatusCode(500, ex.Message); // Handle other internal server errors
            }
        }

        [HttpDelete("{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            try
            {
                var success = await _departmentService.DeleteDepartmentAsync(departmentId);

                if (success)
                {
                    return Ok(); // Success
                }
                return NotFound("Department not found"); // Handle department not found case
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Handle invalid department ID
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Handle other internal server errors
            }
        }
    }
}
