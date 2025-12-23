using Common.DTO.Login;

using XPlan.Service;

namespace Service.Interface
{
    public interface IAuthService : IService<LoginRequest, LoginResponse>
    {
    }
}
