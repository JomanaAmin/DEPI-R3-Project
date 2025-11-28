using Bookify.MVC.Models.AccountModels;

namespace Bookify.MVC.Contracts
{
    public interface IAccountService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<SignupResponseDTO> CreateAccountAdminAsync(SignupRequestDTO signupRequest);
        Task<SignupResponseDTO> CreateAccountCustomerAsync(SignupRequestDTO signupRequest);
        Task<CustomerAccountViewDTO> ViewCustomerProfile();
        Task<AdminAccountViewDTO> ViewAdminProfile();


    }
}
