using Common.DTO.Login;

using XPlan.Service;

namespace Service.Interface
{
    public interface ILoginService : IService<LoginRequest, LoginResponse>
    {
    }
}
