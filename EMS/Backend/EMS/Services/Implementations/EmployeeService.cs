using EMS.DtoMapping;
using EMS.DtoMapping.DTOs.EmployeeDTOs;
using EMS.Helpers;
using EMS.Helpers.ErrorHelper;
using EMS.Models;
using EMS.Repositories.Implementations;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;

namespace EMS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ApiResultFactory _apiResultFactory;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDesignationRepository _designationRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository, 
            ApiResultFactory apiResultFactory,
            IDepartmentRepository departmentRepository
            , IDesignationRepository designationRepository
            )
        {
            _employeeRepository = employeeRepository;
            _apiResultFactory = apiResultFactory;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
        }

        public async Task<ApiResult> GetAllEmployeesAsync()
        {
            try
            {
                var result = await _employeeRepository.GetAllEmployeesAsync();
                //var resultDTOs = EmployeeMapper.ToDTOList(result.Data);
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
        public async Task<ApiResult> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            Employee employee = dto.ToEmployee();
            
            try
            {
                if (!employee.DepartmentId.HasValue)
                {
                    return _apiResultFactory.CreateErrorResult(
                        ErrorCode.BAD_REQUEST,
                        "Department ID is required.",
                        ErrorLayer.Service);
                }

                if (!employee.DesignationId.HasValue)
                {
                    return _apiResultFactory.CreateErrorResult(
                        ErrorCode.BAD_REQUEST,
                        "Designation ID is required.",
                        ErrorLayer.Service);
                }

                var department = await _departmentRepository.GetDepartmentByIdAsync(employee.DepartmentId.Value);

                if (department.Data == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR, 
                        "Department not found or deleted", ErrorLayer.Service);
                }

                var designation = await _designationRepository.GetDesignationByIdAsync(employee.DesignationId.Value);

                if (designation.Data == null)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.NOT_FOUND_ERROR,
                        "Designation not found or deleted", ErrorLayer.Service);
                }


                var result = await _employeeRepository.CreateEmployeeAsync(employee);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, 
                    ErrorMessage.CREATE_EMPLOYEE_ERROR, ErrorLayer.Service);
            }
        }

        public async Task<ApiResult> UpdateEmployeeInformationAsync(int employeeId, UpdateEmployeeDto dto)
        {
            Employee employee = dto.ToEmployee(employeeId);
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
