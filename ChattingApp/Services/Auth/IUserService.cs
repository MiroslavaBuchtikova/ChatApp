using ChattingApp.Model;

namespace ChattingApp.Services.Auth
{
    public interface IUserService
    {
        Task<AuthResult> LoginAsync(LoginModel login);
        Task<AuthResult> RegisterUser(RegisterModel registration);
    }
}
