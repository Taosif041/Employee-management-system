using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Models;
using EMS.Repositories.Interfaces;
using EMS.Helpers;
using EMS.EMS.Repositories.DatabaseProviders.Interfaces;
using static EMS.Helpers.Enums;
using EMS.Helpers.ErrorHelper;

namespace EMS.Repositories.Implementations
{
    public class OperationLogRepository : IOperationLogRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly ApiResultFactory _apiResultFactory;
        private readonly IMongoCollection<OperationLog> _operationLogCollection;

        public OperationLogRepository(IDatabaseFactory databaseFactory, ApiResultFactory apiResultFactory)
        {
            _databaseFactory = databaseFactory ?? throw new ArgumentNullException(nameof(databaseFactory));
            _operationLogCollection = _databaseFactory.CreateMongoDbConnection().GetCollection<OperationLog>("OperationLogs");
            _apiResultFactory = apiResultFactory;
        }

        public async Task<ApiResult> LogOperationAsync(OperationLog log)
        {
            if (log == null)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, ErrorMessage.CREATE_LOG_ERROR);
            }

            try
            {
                await _operationLogCollection.InsertOneAsync(log);
                return _apiResultFactory.CreateSuccessResult("Log creation successful");
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.CREATE_LOG_ERROR);
            }
        }

        public async Task<ApiResult> GetLogsWithParametersAsync(OperationType? operationType = null,
                                                                                 EntityName? entityName = null,
                                                                                 int? entityId = null)
        {
            try
            {
                if (!operationType.HasValue && !entityName.HasValue && !entityId.HasValue)
                {
                    return _apiResultFactory.CreateErrorResult(ErrorCode.BAD_REQUEST, "No parameters provided to filter logs.");
                }

                var filter = Builders<OperationLog>.Filter.Empty;

                if (operationType.HasValue)
                {
                    filter &= Builders<OperationLog>.Filter.Eq(log => log.OperationType, operationType.Value.ToString());
                }

                if (entityName.HasValue)
                {
                    filter &= Builders<OperationLog>.Filter.Eq(log => log.EntityName, entityName.Value.ToString());
                }

                if (entityId.HasValue)
                {
                    filter &= Builders<OperationLog>.Filter.Eq(log => log.EntityId, entityId.Value);
                }

                var result = await _operationLogCollection.Find(filter).ToListAsync();
                return _apiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_LOG_ERROR);
            }
        }

        public async Task<ApiResult> GetAllLogsAsync()
        {
            try
            {
                var result = await _operationLogCollection.Find(_ => true).ToListAsync();
                
                return _apiResultFactory.CreateSuccessResult(result);
            }
            catch (Exception ex)
            {
                return _apiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.GET_LOG_ERROR);
            }
        }


    }


}
