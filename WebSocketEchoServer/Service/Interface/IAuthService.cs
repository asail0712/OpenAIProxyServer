using Microsoft.AspNetCore.Mvc;
using Service.DTO.Auth;

using XPlan.Service;

namespace Service.Interface
{
    public interface IAuthService : IService<LoginRequest, LoginResponse>
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<bool> ChangePassword(ChangePasswordRequest request);
    }
}
