using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepository _designationRepository;

        public DesignationService(IDesignationRepository designationRepository)
        {
            _designationRepository = designationRepository ?? throw new ArgumentNullException(nameof(designationRepository), "Designation repository cannot be null.");
        }

        public async Task<IEnumerable<Designation>> GetAllDesignationsAsync()
        {
            try
            {
                var designations = await _designationRepository.GetAllDesignationsAsync();

                // Log the designations (for debugging purposes)
                Console.WriteLine("Designations fetched: ");
                //foreach (var designation in designations)
                //{
                //    Console.WriteLine($"DesignationId: {designation.DesignationId}, Name: {designation.Name}");
                //}

                return designations;
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error fetching designations: {ex.Message}");

                // Rethrow the exception or handle gracefully
                throw new Exception("An error occurred while retrieving the designations.", ex);
            }
        }

        public async Task<Designation> GetDesignationByIdAsync(int designationId)
        {
            try
            {
                return await _designationRepository.GetDesignationByIdAsync(designationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching designation by ID: {ex.Message}");
                throw new Exception($"An error occurred while retrieving the designation with ID {designationId}.", ex);
            }
        }

        public async Task<Designation> CreateDesignationAsync(Designation designation)
        {
            try
            {
                if (designation == null)
                {
                    throw new ArgumentNullException(nameof(designation), "Designation cannot be null.");
                }

                return await _designationRepository.CreateDesignationAsync(designation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating designation: {ex.Message}");
                throw new Exception("An error occurred while creating the designation.", ex);
            }
        }

        public async Task<Designation> UpdateDesignationAsync(Designation designation)
        {
            try
            {
                if (designation == null)
                {
                    throw new ArgumentNullException(nameof(designation), "Designation cannot be null.");
                }

                return await _designationRepository.UpdateDesignationAsync(designation);
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("Designation cannot be null while updating in service layer.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating designation: {ex.Message}");
                throw new Exception("An error occurred while updating the designation.", ex);
            }
        }

        public async Task<bool> DeleteDesignationAsync(int designationId)
        {
            try
            {
                if (designationId <= 0)
                {
                    throw new ArgumentException("Invalid designation ID.");
                }

                return await _designationRepository.DeleteDesignationAsync(designationId);
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Invalid designation ID for deletion.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting designation: {ex.Message}");
                throw new Exception("An error occurred while deleting the designation.", ex);
            }
        }
    }
}
