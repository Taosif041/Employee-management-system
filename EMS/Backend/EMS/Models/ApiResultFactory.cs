namespace EMS.Models
{
    public class ApiResultFactory
    {
        public ApiResult CreateErrorResult(int errorCode, string errorMessage, List<string> errors =null)
        {
            return new ApiResult
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
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
