namespace GravyFoodsApi.Models.DTOs
{
    public class APIResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }


    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }


}
