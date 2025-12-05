
using Bookify.MVC.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Services
{
    public class AccountMVCService
    {
        private readonly HttpClient httpClient;
        public AccountMVCService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest) 
        {
            var response = await httpClient.PostAsJsonAsync("login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                return data;
            }
           
            return null;
            
        }
        public async Task<RequestResult<object>> RegisterCustomerAsync(SignupRequestDTO signupRequest) 
        {
            var response = await httpClient.PostAsJsonAsync("account/register-customer", signupRequest);

            if (response.IsSuccessStatusCode)
            {
                return new RequestResult<object> { Success = true };
            }

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            return new RequestResult<object>
            {
                Success = false,
                ErrorMessage = problem?.Detail ?? "An unknown error occurred",
                Data=null
            };
        }
    }
}
