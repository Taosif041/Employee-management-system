using EMS.Core.Helpers;
using EMS.Helpers;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ApiResultFactory _apiResultFactory;

        public DepartmentService(IDepartmentRepository departmentRepository, ApiResultFactory apiResultFactory)
        {
            _departmentRepository = departmentRepository;
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> GetAllDepartmentsAsync()
        {
            try
            {
                var result = await _departmentRepository.GetAllDepartmentsAsync();
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR);
            }
        }

        public async Task<ApiResult> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                var result = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_DEPARTMENT_ERROR);
            }
        }

        public async Task<ApiResult> CreateDepartmentAsync(Department department)
        {
            try
            {
                if (department == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, "Department cannot be null.");
                }

                var result = await _departmentRepository.CreateDepartmentAsync(department);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_DEPARTMENT_ERROR);
            }
        }

        public async Task<ApiResult> UpdateDepartmentAsync(Department department)
        {
            try
            {
                if (department == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, "Department cannot be null.");
                }

                var result = await _departmentRepository.UpdateDepartmentAsync(department);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.UPDATE_DEPARTMENT_ERROR);
            }
        }

        public async Task<ApiResult> DeleteDepartmentAsync(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, "Department cannot be null.");
                }

                var result = await _departmentRepository.DeleteDepartmentAsync(departmentId);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.DELETE_DEPARTMENT_ERROR);
            }
        }
    }
}
