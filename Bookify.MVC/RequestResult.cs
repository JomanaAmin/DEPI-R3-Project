namespace Bookify.MVC
{
    public class RequestResult<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
    }

}
