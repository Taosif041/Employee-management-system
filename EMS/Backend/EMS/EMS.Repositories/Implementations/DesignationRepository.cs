using Dapper;
using Microsoft.Data.SqlClient;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using static EMS.Data.Enums;

namespace EMS.Repositories.Implementations
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly SqlConnection _connection;
        private readonly OperationLogger _operationLogger;

        public DesignationRepository(SqlConnection connection, IOperationLogRepository operationLogRepository)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection), "Database connection cannot be null.");
            _operationLogger = new OperationLogger(operationLogRepository);
        }

        public async Task<IEnumerable<Designation>> GetAllDesignationsAsync()
        {
            try
            {
                await _connection.OpenAsync();
                var designations = await _connection.QueryAsync<Designation>("GetAllDesignations", commandType: CommandType.StoredProcedure);

                await _operationLogger.LogOperationAsync(EntityName.Designation, 0, OperationType.GetAll);

                return designations;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("An error occurred while fetching designations from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while fetching designations.", ex);
            }
        }

        public async Task<Designation> GetDesignationByIdAsync(int designationId)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new { DesignationId = designationId };
                var designation = await _connection.QueryFirstOrDefaultAsync<Designation>("GetDesignationById", parameters, commandType: CommandType.StoredProcedure);

                await _operationLogger.LogOperationAsync(EntityName.Designation, designationId, OperationType.GetById);

                return designation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while fetching the designation by ID.", ex);
            }
        }

        public async Task<Designation> CreateDesignationAsync(Designation designation)
        {
            if (designation == null)
                throw new ArgumentNullException(nameof(designation), "Designation cannot be null.");

            try
            {
                await _connection.OpenAsync();

                var parameters = new
                {
                    designation.Name
                };

                var newId = await _connection.ExecuteScalarAsync<int>("CreateDesignation", parameters, commandType: CommandType.StoredProcedure);
                designation.DesignationId = newId;

                await _operationLogger.LogOperationAsync(EntityName.Designation, newId, OperationType.Create);

                return designation;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("An error occurred while creating the designation in the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while creating the designation.", ex);
            }
        }

        public async Task<Designation> UpdateDesignationAsync(Designation designation)
        {
            if (designation == null)
                throw new ArgumentNullException(nameof(designation), "Designation object cannot be null.");

            try
            {
                // Retrieve the current designation data
                var currentDesignation = await GetDesignationByIdAsync(designation.DesignationId);
                if (currentDesignation == null)
                    throw new Exception($"Designation with ID {designation.DesignationId} not found.");

                var parameters = new
                {
                    DesignationId = designation.DesignationId,
                    Name = designation.Name ?? currentDesignation.Name
                };

                await _connection.ExecuteAsync("UpdateDesignation", parameters, commandType: CommandType.StoredProcedure);

                await _operationLogger.LogOperationAsync(EntityName.Designation, designation.DesignationId, OperationType.Update);

                return designation;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("An error occurred while updating designation information in the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while updating designation information.", ex);
            }
        }

        public async Task<bool> DeleteDesignationAsync(int designationId)
        {
            try
            {
                await _connection.OpenAsync();
                var parameters = new { DesignationId = designationId };

                var rowsAffected = await _connection.ExecuteAsync("DeleteDesignation", parameters, commandType: CommandType.StoredProcedure);

                await _operationLogger.LogOperationAsync(EntityName.Designation, designationId, OperationType.Delete);

                return rowsAffected > 0;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("An error occurred while deleting designation information from the database.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An unexpected error occurred while deleting designation information.", ex);
            }
        }
    }
}
