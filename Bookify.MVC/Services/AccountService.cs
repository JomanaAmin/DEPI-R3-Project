using Bookify.MVC.Contracts;
using Bookify.MVC.Models;
using Bookify.MVC.Models.AccountModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace Bookify.MVC.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequest)
        {
            var response = await httpClient.PostAsJsonAsync("/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorDTO>();
                return ApiResponse<LoginResponseDTO>.Fail(error);
            }

            var data = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            return ApiResponse<LoginResponseDTO>.Success(data);
        }

        public async Task<SignupResponseDTO> CreateAccountAdminAsync(SignupRequestDTO signupRequest)
        {
            var response = await httpClient.PostAsJsonAsync("Account/register-admin", signupRequest);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = "Account creation failed. Please try again.";
                var errorDto = await response.Content.ReadFromJsonAsync<AccountErrorDTO>();
                if (errorDto?.Message != null)
                {
                    errorMessage = errorDto.Message;
                }
                return new SignupResponseDTO
                {
                    IsSuccessful = false,
                    ValidationMessage = errorMessage
                };
            }

            // Account created successfully — attempt to log the user in immediately
            var loginResult = await LoginAsync(new LoginRequestDTO { Username = signupRequest.Email, Password = signupRequest.Password });
            if (loginResult != null && loginResult.IsSuccessful)
            {
                return new SignupResponseDTO
                {
                    IsSuccessful = true,
                    ValidationMessage = "Account created and logged in.",
                    AccessToken = loginResult.AccessToken,
                    Expiration = loginResult.Expiration
                };
            }

            // Account created but automatic login failed
            return new SignupResponseDTO
            {
                IsSuccessful = false,
                ValidationMessage = "Account created but automatic login failed: " + (loginResult?.ValidationMessage ?? "unknown error")
            };
        }
        public async Task<SignupResponseDTO> CreateAccountCustomerAsync(SignupRequestDTO signupRequest)
        {
            var response = await httpClient.PostAsJsonAsync("Account/register-customer", signupRequest);
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = "Account creation failed. Please try again.";
                var errorDto = await response.Content.ReadFromJsonAsync<AccountErrorDTO>();
                if (errorDto?.Message != null)
                {
                    errorMessage = errorDto.Message;
                }
                return new SignupResponseDTO
                {
                    IsSuccessful = false,
                    ValidationMessage = errorMessage
                };
            }

            // Account created successfully — attempt to log the user in immediately
            var loginResult = await LoginAsync(new LoginRequestDTO { Username = signupRequest.Email, Password = signupRequest.Password });
            if (loginResult != null && loginResult.IsSuccessful)
            {
                return new SignupResponseDTO
                {
                    IsSuccessful = true,
                    ValidationMessage = "Account created and logged in.",
                    AccessToken = loginResult.AccessToken,
                    Expiration = loginResult.Expiration
                };
            }

            // Account created but automatic login failed
            return new SignupResponseDTO
            {
                IsSuccessful = false,
                ValidationMessage = "Account created but automatic login failed: " + (loginResult?.ValidationMessage ?? "unknown error")
            };
        }
        public async Task<CustomerAccountViewDTO> ViewCustomerProfile()
        {
            // 1. Send the secured GET request. 
            // The AuthTokenHandler (registered earlier) automatically adds the JWT from the cookie.
            var response = await httpClient.GetAsync("Account/customer-profile");

            if (response.IsSuccessStatusCode)
            {
                // 2. Success: Read and return the profile data
                var profileDto = await response.Content.ReadFromJsonAsync<CustomerAccountViewDTO>();

                // You might add a success flag or message to AccountViewDTO if needed, 
                // but for a successful GET, returning the data is enough.
                return profileDto ?? new CustomerAccountViewDTO
                {
                    Email = profileDto?.Email ?? string.Empty,
                    IsSuccessful = true,
                    FirstName = profileDto?.FirstName ?? string.Empty,
                    LastName = profileDto?.LastName ?? string.Empty
                };
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                // 3. Handle Token Failure (401/403)
                // This usually means the token is missing, expired, or doesn't have permissions.
                // Since the token handler handles adding the token, an error here means the user is logged out.
                // You would typically throw a specific exception here to trigger a redirect to the login page 
                // in the MVC Controller.
                throw new AuthenticationException("Your session has expired. Please log in again.");
            }
            else
            {
                // 4. Handle other failures (404, 500)
                // For security, read the generic error if provided by the API
                var errorDto = await response.Content.ReadFromJsonAsync<AccountErrorDTO>();
                string errorMessage = errorDto?.Message ?? "Could not load profile due to a server error.";
                return new CustomerAccountViewDTO
                {
                    // You might want to add an ErrorMessage property to AccountViewDTO to convey this.
                    ValidationMessage = errorMessage,
                    IsSuccessful = false,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = string.Empty
                };
            }
        }
        public async Task<AdminAccountViewDTO> ViewAdminProfile()
        {
            // 1. Send the secured GET request. 
            // The AuthTokenHandler (registered earlier) automatically adds the JWT from the cookie.
            var response = await httpClient.GetAsync("Account/admin-profile");

            if (response.IsSuccessStatusCode)
            {
                // 2. Success: Read and return the profile data
                var profileDto = await response.Content.ReadFromJsonAsync<AdminAccountViewDTO>();

                // You might add a success flag or message to AccountViewDTO if needed, 
                // but for a successful GET, returning the data is enough.
                return profileDto ?? new AdminAccountViewDTO
                {
                    Email = profileDto?.Email ?? string.Empty,
                    IsSuccessful = true,
                    FirstName = profileDto?.FirstName ?? string.Empty,
                    LastName = profileDto?.LastName ?? string.Empty
                };
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                // 3. Handle Token Failure (401/403)
                // This usually means the token is missing, expired, or doesn't have permissions.
                // Since the token handler handles adding the token, an error here means the user is logged out.
                // You would typically throw a specific exception here to trigger a redirect to the login page 
                // in the MVC Controller.
                throw new AuthenticationException("Your session has expired. Please log in again.");
            }
            else
            {
                // 4. Handle other failures (404, 500)
                // For security, read the generic error if provided by the API
                var errorDto = await response.Content.ReadFromJsonAsync<AccountErrorDTO>();
                string errorMessage = errorDto?.Message ?? "Could not load profile due to a server error.";
                return new AdminAccountViewDTO
                {
                    // You might want to add an ErrorMessage property to AccountViewDTO to convey this.
                    ValidationMessage = errorMessage,
                    IsSuccessful = false,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = string.Empty
                };
            }
        }
    }
}