namespace Bookify.MVC.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public ErrorDTO Error { get; set; }

        public static ApiResponse<T> Success(T data)
            => new ApiResponse<T> { IsSuccess = true, Data = data };

        public static ApiResponse<T> Fail(ErrorDTO error)
            => new ApiResponse<T> { IsSuccess = false, Error = error };
    }

}
