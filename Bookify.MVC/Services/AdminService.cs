using Bookify.MVC.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.MVC.Services
{
    public class AdminService
    {
        private readonly HttpClient httpClient;
        public AdminService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

      
        public async Task<RequestResult> RegisterAdminAsync(SignupRequestDTO signupRequest)
        {
            var response = await httpClient.PostAsJsonAsync("account/register-admin", signupRequest);

            if (response.IsSuccessStatusCode)
            {
                return new RequestResult { Success = true };
            }

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            return new RequestResult
            {
                Success = false,
                ErrorMessage = problem?.Detail ?? "An unknown error occurred"
            };
        }
    }
}

