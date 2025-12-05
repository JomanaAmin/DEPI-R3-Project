using Bookify.MVC.Models.CartDTO;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<RequestResult<CartViewDTO>> ViewCartAsync()
        {
            var response = await httpClient.GetAsync("cart");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<CartViewDTO>();
                return new RequestResult<CartViewDTO> 
                {
                    Success=true,
                    Data=data
                };

            }
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            return new RequestResult<CartViewDTO>
            {
                Success=false,
                ErrorMessage=problem?.Detail??"An unknown error occurred."
            };

        }
        public async Task<RequestResult<object>> AddToCartAsync(CartAddItemDTO cartAddItem)
        {
            var response = await httpClient.PostAsJsonAsync("cart", cartAddItem);
            if (response.IsSuccessStatusCode)
            {
                return new RequestResult<object>
                {
                    Success = true,
                    ErrorMessage = "Item added to cart successfully!" // pop up msg
                };

            }
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            return new RequestResult<object>
            {
                Success = false,
                ErrorMessage = problem?.Detail ?? "An unknown error occurred."
            };

        }

    }
}
