namespace Bookify.MVC.Services
{
    public class CartMVCService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<CartMVCService> logger;
        public CartMVCService(HttpClient httpClient, ILogger<CartMVCService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }
        
    }
}
