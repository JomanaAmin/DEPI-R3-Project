using Bookify.MVC.Models;
using Bookify.MVC.Models.AccountModels;

namespace Bookify.MVC.Contracts
{
    public interface IAccountService
    {
        Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ApiResponse<SignupResponseDTO>> CreateAccountAdminAsync(SignupRequestDTO signupRequest);
        Task<ApiResponse<SignupResponseDTO>> CreateAccountCustomerAsync(SignupRequestDTO signupRequest);
        Task<ApiResponse<CustomerAccountViewDTO>> ViewCustomerProfile();
        Task<ApiResponse<AdminAccountViewDTO>> ViewAdminProfile();


    }
}
