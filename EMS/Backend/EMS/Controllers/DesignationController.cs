using EMS.Models;
using EMS.Services.Interfaces;
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

        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService ?? throw new ArgumentNullException(nameof(designationService), "Designation service cannot be null.");
        }

        // GET: api/designation
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var designations = await _designationService.GetAllDesignationsAsync();
                if (designations == null)
                {
                    return NotFound("No designations found.");
                }
                return Ok(designations); // Return list of all designations
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching designations.");
            }
        }

        // GET: api/designation/{id}
        [HttpGet("{designationId}")]
        public async Task<IActionResult> GetDesignationByIdAsync(int designationId)
        {
            try
            {
                var designation = await _designationService.GetDesignationByIdAsync(designationId);
                if (designation == null)
                {
                    return NotFound($"Designation with ID {designationId} not found.");
                }

                return Ok(designation); // Return the designation
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"An error occurred while fetching designation with ID {designationId}.");
            }
        }

        // POST: api/designation
        [HttpPost]
        public async Task<IActionResult> CreateDesignationAsync([FromBody] Designation designation)
        {
            try
            {
                if (designation == null)
                {
                    return BadRequest("Designation cannot be null.");
                }

                var createdDesignation = await _designationService.CreateDesignationAsync(designation);
                return Ok(createdDesignation); // Return the newly created designation
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid data: {ex.Message}"); // Handle invalid input
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the designation.");
            }
        }

        // PUT: api/designation/{id}
        [HttpPut("{designationId}")]
        public async Task<IActionResult> UpdateDesignationAsync(int designationId, [FromBody] Designation designation)
        {
            try
            {
                if (designationId != designation.DesignationId)
                {
                    return BadRequest("Designation ID mismatch.");
                }

                var updatedDesignation = await _designationService.UpdateDesignationAsync(designation);
                if (updatedDesignation == null)
                {
                    return NotFound($"Designation with ID {designationId} not found.");
                }

                return Ok(updatedDesignation); // Return the updated designation
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"Invalid data: {ex.Message}"); // Handle invalid input
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the designation.");
            }
        }

        // DELETE: api/designation/{id}
        [HttpDelete("{designationId}")]
        public async Task<IActionResult> DeleteDesignationAsync(int designationId)
        {
            try
            {
                var success = await _designationService.DeleteDesignationAsync(designationId);
                if (success)
                {
                    return Ok();
                }
                return NotFound($"Designation with ID {designationId} not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid data: {ex.Message}"); // Handle invalid input
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the designation.");
            }
        }
    }
}
