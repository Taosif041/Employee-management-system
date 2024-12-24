using EMS.Core.Helpers;
using EMS.Helpers;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EMS.Helpers.Enums;

namespace EMS.Services.Implementations
{
    public class OperationLogService : IOperationLogService
    {
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly ApiResultFactory _apiResultFactory;

        public OperationLogService(IOperationLogRepository operationLogRepository, ApiResultFactory apiResultFactory)
        {
            _operationLogRepository = operationLogRepository;
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> GetAllLogsAsync()
        {
            try
            {
                var result = await _operationLogRepository.GetAllLogsAsync();
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR,
                    ErrorMessage.GET_LOG_ERROR);
            }
        }

        public async Task<ApiResult> GetLogsWithParametersAsync(
            OperationType? operationType = null,
            EntityName? entityName = null,
            int? entityId = null)
        {
            try
            {
                var result = await _operationLogRepository.GetLogsWithParametersAsync(operationType, entityName, entityId);
                return result;
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR,
                    ErrorMessage.GET_LOG_ERROR);
            }
        }

        

    }
}
