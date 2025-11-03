using AutoMapper;
using Common.DTO.Login;
using Repository.Interface;
using Service.Interface;
using XPlan.Service;

namespace Service
{
    public class LoginService : GenericService<LoginEntity, LoginRequest, LoginResponse, ILoginRepository>, ILoginService
    {
        public LoginService(ILoginRepository repo, IMapper mapper) 
            : base(repo, mapper)
        {
        }
    }
}
