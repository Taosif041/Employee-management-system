using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;

namespace EMS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ApiResultFactory _apiResultFactory;

        public EmployeeService(IEmployeeRepository employeeRepository, ApiResultFactory apiResultFactory)
        {
            _employeeRepository = employeeRepository;
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> GetAllEmployeesAsync()
        {
            try
            {
                var result = await _employeeRepository.GetAllEmployeesAsync();
                var resultDTOs = EmployeeMapper.ToDTOList(result.Data);
                return result; 
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.GET_EMPLOYEE_ERROR, ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                var result = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
                return result;

            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.GET_EMPLOYEE_ERROR, ErrorLayer.Service);
            }
        }
        public async Task<ApiResult> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                var result = await _employeeRepository.CreateEmployeeAsync(employee);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.CREATE_EMPLOYEE_ERROR, ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> UpdateEmployeeInformationAsync(Employee employee)
        {
            try
            {
                var result =  await _employeeRepository.UpdateEmployeeInformationAsync(employee);
                return result;
            }

            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.UPDATE_EMPLOYEE_ERROR, ErrorLayer.Service);
            }
        }
        public async Task<ApiResult> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                var result = await _employeeRepository.DeleteEmployeeAsync(employeeId);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.DELETE_EMPLOYEE_ERROR, ErrorLayer.Service);
            }
        }






    }
}
