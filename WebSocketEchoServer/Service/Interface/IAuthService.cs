using Microsoft.AspNetCore.Mvc;
using Service.DTO.Auth;

using XPlan.Service;

namespace Service.Interface
{
    public interface IAuthService : IService<IdentityRequest, LoginResponse>
    {
        Task<LoginResponse> Login(IdentityRequest request);
        Task<bool> CreateIdentity(IdentityRequest request);
        Task<bool> ChangePassword(ChangePasswordRequest request);
    }
}
