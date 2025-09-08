namespace GravyFoodsApi.Models
{

    // Standard wrapper for service results
    public class ServiceResultWrapper<T>
    {
        public bool Success { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }

        public static ServiceResultWrapper<T> Ok(T data) => new ServiceResultWrapper<T> { Success = true, Data = data };
        public static ServiceResultWrapper<T> Fail(string message) => new ServiceResultWrapper<T> { Success = false, Message = message };
    }
}
