using Bookify.MVC.Contracts;
using Bookify.MVC.Models;
using Bookify.MVC.Models.AccountModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace Bookify.MVC.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<AccountService> logger;

        public AccountService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<AccountService> logger)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private async Task<ErrorDTO> ReadErrorAsync(HttpResponseMessage? response)
        {
            if (response == null) return new ErrorDTO { Message = "No response received from API." };

            try
            {
                if (response.Content == null) return new ErrorDTO { Message = $"API returned status {(int)response.StatusCode} {response.StatusCode} with no content." };

                var raw = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(raw))
                {
                    return new ErrorDTO { Message = $"API returned status {(int)response.StatusCode} {response.StatusCode} with empty body." };
                }

                // Try to parse JSON error first
                try
                {
                    var parsed = JsonSerializer.Deserialize<ErrorDTO>(raw, JsonOptions);
                    if (parsed != null && !string.IsNullOrWhiteSpace(parsed.Message))
                    {
                        return parsed;
                    }
                }
                catch (JsonException)
                {
                    // fallthrough to return raw body as message
                }

                // If not JSON, return the raw body as message (guards against HTML error pages etc.)
                return new ErrorDTO { Message = raw };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to read error response from API");
                return new ErrorDTO { Message = $"Unable to read error response: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequest)
        {
            logger.LogInformation("LoginAsync called. Email: {Email}", loginRequest?.Username);

            try
            {
                logger.LogDebug("Posting to {Base}{Path} with payload {@Payload}", httpClient.BaseAddress, "login", loginRequest);
                var response = await httpClient.PostAsJsonAsync("login", loginRequest);

                if (response == null)
                {
                    logger.LogError("HttpClient.PostAsJsonAsync returned null response");
                    return ApiResponse<LoginResponseDTO>.Fail(new ErrorDTO { Message = "No response from API." });
                }

                logger.LogDebug("Received status code {Status}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Content?.Headers.ContentLength > 0)
                    {
                        try
                        {
                            var data = await response.Content.ReadFromJsonAsync<LoginResponseDTO>(JsonOptions);
                            if (data != null)
                            {
                                logger.LogInformation("Login succeeded for {Email}", loginRequest?.Username);
                                return ApiResponse<LoginResponseDTO>.Success(data);
                            }

                            logger.LogWarning("Deserialized login response was null for {Email}", loginRequest?.Username);
                        }
                        catch (JsonException ex)
                        {
                            // Content not valid JSON — capture raw and return helpful message
                            var raw = await response.Content.ReadAsStringAsync();
                            logger.LogError(ex, "Failed to deserialize login success payload. Raw content: {Raw}", raw);
                            return ApiResponse<LoginResponseDTO>.Fail(new ErrorDTO { Message = $"Unexpected response format: {raw}" });
                        }
                    }
                    else
                    {
                        logger.LogWarning("Login returned success status but empty content for {Email}", loginRequest?.Username);
                    }

                    return ApiResponse<LoginResponseDTO>.Fail(new ErrorDTO { Message = "API returned success but no authentication data was received." });
                }
                else
                {
                    var error = await ReadErrorAsync(response);
                    logger.LogWarning("Login failed for {Email}: {Message}", loginRequest?.Username, error.Message);
                    return ApiResponse<LoginResponseDTO>.Fail(error);
                }
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Network error calling login endpoint for {Email}", loginRequest?.Username);
                return ApiResponse<LoginResponseDTO>.Fail(new ErrorDTO { Message = $"Network error calling API: {ex.Message}" });
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "JSON (de)serialization error in LoginAsync for {Email}", loginRequest?.Username);
                return ApiResponse<LoginResponseDTO>.Fail(new ErrorDTO { Message = $"Serialization error: {ex.Message}" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in LoginAsync for {Email}", loginRequest?.Username);
                return ApiResponse<LoginResponseDTO>.Fail(new ErrorDTO { Message = $"Unexpected error calling API: {ex.Message}" });
            }
        }

        public async Task<ApiResponse<SignupResponseDTO>> CreateAccountAdminAsync(SignupRequestDTO signupRequest)
        {
            logger.LogDebug("CreateAccountAdminAsync called for {Email}", signupRequest?.Email);
            try
            {
                var response = await httpClient.PostAsJsonAsync("/account/register-admin", signupRequest);
                if (response == null)
                {
                    logger.LogError("No response from CreateAccountAdminAsync");
                    return ApiResponse<SignupResponseDTO>.Fail(new ErrorDTO { Message = "No response from API." });
                }

                if (!response.IsSuccessStatusCode)
                {
                    var error = await ReadErrorAsync(response);
                    return ApiResponse<SignupResponseDTO>.Fail(error);
                }

                try
                {
                    var data = await response.Content.ReadFromJsonAsync<SignupResponseDTO>(JsonOptions);
                    if (data == null)
                    {
                        logger.LogWarning("CreateAccountAdminAsync success payload was null");
                        return ApiResponse<SignupResponseDTO>.Fail(new ErrorDTO { Message = "API returned success but no data." });
                    }
                    return ApiResponse<SignupResponseDTO>.Success(data);
                }
                catch (JsonException ex)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    logger.LogError(ex, "Failed to deserialize signup success payload. Raw: {Raw}", raw);
                    return ApiResponse<SignupResponseDTO>.Fail(new ErrorDTO { Message = $"Unexpected response format: {raw}" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in CreateAccountAdminAsync");
                return ApiResponse<SignupResponseDTO>.Fail(new ErrorDTO { Message = ex.Message });
            }
        }

        public async Task<ApiResponse<SignupResponseDTO>> CreateAccountCustomerAsync(SignupRequestDTO signupRequest)
        {
            logger.LogDebug("CreateAccountCustomerAsync called for {Email}", signupRequest?.Email);
            try
            {
                var response = await httpClient.PostAsJsonAsync("/account/register-admin", signupRequest);
                if (response == null)
                {
                    logger.LogError("No response from CreateAccountCustomerAsync");
                    return ApiResponse<SignupResponseDTO>.Fail(new ErrorDTO { Message = "No response from API." });
                }

                if (!response.IsSuccessStatusCode)
                {
                    var error = await ReadErrorAsync(response);
                    return ApiResponse<SignupResponseDTO>.Fail(error);
                }

                try
                {
                    var data = await response.Content.ReadFromJsonAsync<SignupResponseDTO>(JsonOptions);
                    if (data == null)
                    {
                        logger.LogWarning("CreateAccountCustomerAsync success payload was null");
                        return ApiResponse<SignupResponseDTO>.Fail(new ErrorDTO { Message = "API returned success but no data." });
                    }
                    return ApiResponse<SignupResponseDTO>.Success(data);
                }
                catch (JsonException ex)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    logger.LogError(ex, "Failed to deserialize signup (customer) success payload. Raw: {Raw}", raw);
                    return ApiResponse<SignupResponseDTO>.Fail(new ErrorDTO { Message = $"Unexpected response format: {raw}" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in CreateAccountCustomerAsync");
                return ApiResponse<SignupResponseDTO>.Fail(new ErrorDTO { Message = ex.Message });
            }
        }

        public async Task<ApiResponse<CustomerAccountViewDTO>> ViewCustomerProfile()
        {
            logger.LogDebug("ViewCustomerProfile called");
            try
            {
                var response = await httpClient.GetAsync("account/customer-profile");
                if (response == null)
                {
                    logger.LogError("No response from ViewCustomerProfile");
                    return ApiResponse<CustomerAccountViewDTO>.Fail(new ErrorDTO { Message = "No response from API." });
                }

                if (!response.IsSuccessStatusCode)
                {
                    var error = await ReadErrorAsync(response);
                    return ApiResponse<CustomerAccountViewDTO>.Fail(error);
                }

                try
                {
                    var data = await response.Content.ReadFromJsonAsync<CustomerAccountViewDTO>(JsonOptions);
                    if (data == null)
                    {
                        logger.LogWarning("ViewCustomerProfile returned null data");
                        return ApiResponse<CustomerAccountViewDTO>.Fail(new ErrorDTO { Message = "API returned success but no data." });
                    }
                    return ApiResponse<CustomerAccountViewDTO>.Success(data);
                }
                catch (JsonException ex)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    logger.LogError(ex, "Failed to deserialize customer profile payload. Raw: {Raw}", raw);
                    return ApiResponse<CustomerAccountViewDTO>.Fail(new ErrorDTO { Message = $"Unexpected response format: {raw}" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in ViewCustomerProfile");
                return ApiResponse<CustomerAccountViewDTO>.Fail(new ErrorDTO { Message = ex.Message });
            }
        }

        public async Task<ApiResponse<AdminAccountViewDTO>> ViewAdminProfile()
        {
            logger.LogDebug("ViewAdminProfile called");
            try
            {
                var response = await httpClient.GetAsync("account/admin-profile");
                if (response == null)
                {
                    logger.LogError("No response from ViewAdminProfile");
                    return ApiResponse<AdminAccountViewDTO>.Fail(new ErrorDTO { Message = "No response from API." });
                }

                if (!response.IsSuccessStatusCode)
                {
                    var error = await ReadErrorAsync(response);
                    return ApiResponse<AdminAccountViewDTO>.Fail(error);
                }

                try
                {
                    var data = await response.Content.ReadFromJsonAsync<AdminAccountViewDTO>(JsonOptions);
                    if (data == null)
                    {
                        logger.LogWarning("ViewAdminProfile returned null data");
                        return ApiResponse<AdminAccountViewDTO>.Fail(new ErrorDTO { Message = "API returned success but no data." });
                    }
                    return ApiResponse<AdminAccountViewDTO>.Success(data);
                }
                catch (JsonException ex)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    logger.LogError(ex, "Failed to deserialize admin profile payload. Raw: {Raw}", raw);
                    return ApiResponse<AdminAccountViewDTO>.Fail(new ErrorDTO { Message = $"Unexpected response format: {raw}" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in ViewAdminProfile");
                return ApiResponse<AdminAccountViewDTO>.Fail(new ErrorDTO { Message = ex.Message });
            }
        }
    }
}