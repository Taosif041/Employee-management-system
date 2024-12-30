using EMS.Models;

namespace EMS.Helpers
{
    public class ApiResultFactory
    {
        public ApiResult CreateErrorResult(int errorCode, string errorMessage, string errorLayer = null, List<string> errors = null)
        {
            return new ApiResult
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                ErrorLayer = errorLayer,
                Errors = errors
            };
        }

        public ApiResult CreateSuccessResult(dynamic? result = null)
        {
            return new ApiResult
            {
                IsSuccess = true,
                Data = result
            };
        }
    }
}
