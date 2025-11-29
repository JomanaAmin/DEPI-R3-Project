
using Bookify.MVC.Models.AccountModels;

namespace Bookify.MVC.Services
{
    public class AccountService
    {
        private readonly HttpClient httpClient;
        public AccountService(HttpClient httpClient)
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
        public async Task RegisterCustomerAsync(SignupRequestDTO signupRequest) 
        {
            var response = await httpClient.PostAsJsonAsync("account/register-customer", signupRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("internal error exception");
            }
            return;
        }
    }
}
