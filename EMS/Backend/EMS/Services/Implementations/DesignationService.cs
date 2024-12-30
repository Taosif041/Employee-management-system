using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
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
        private readonly ApiResultFactory _apiResultFactory;

        public DesignationService(IDesignationRepository designationRepository, ApiResultFactory apiResultFactory)
        {
            _designationRepository = designationRepository;
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> GetAllDesignationsAsync()
        {
            try
            {
                var result = await _designationRepository.GetAllDesignationsAsync();
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR);
            }
        }

        public async Task<ApiResult> GetDesignationByIdAsync(int designationId)
        {
            try
            {
                var result = await _designationRepository.GetDesignationByIdAsync(designationId);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DESIGNATION_ERROR);
            }
        }

        public async Task<ApiResult> CreateDesignationAsync(Designation designation)
        {
            try
            {
                if (designation == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, "Designation cannot be null.");
                }

                var result = await _designationRepository.CreateDesignationAsync(designation);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_DESIGNATION_ERROR);
            }
        }

        public async Task<ApiResult> UpdateDesignationAsync(Designation designation)
        {
            try
            {
                if (designation == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, "Designation cannot be null.");
                }

                var result = await _designationRepository.UpdateDesignationAsync(designation);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DESIGNATION_ERROR);
            }
        }

        public async Task<ApiResult> DeleteDesignationAsync(int designationId)
        {
            try
            {
                if (designationId <= 0)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, "Designation cannot be null.");
                }

                return await _designationRepository.DeleteDesignationAsync(designationId);
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DESIGNATION_ERROR);
            }
        }
    }
}
