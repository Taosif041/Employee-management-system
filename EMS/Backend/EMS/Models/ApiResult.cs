namespace EMS.Models
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; } = true;
        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

        public List<string>? Errors { get; set; }    

        public dynamic? Data { get; set; }  
    }
}
