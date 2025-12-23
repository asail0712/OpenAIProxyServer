using AutoMapper;
using Common.DTO.Login;
using Repository.Interface;
using Service.Interface;
using XPlan.Service;

namespace Service
{
    public class AuthService : GenericService<AuthEntity, LoginRequest, LoginResponse, IAuthRepository>, IAuthService
    {
        public AuthService(IAuthRepository repo, IMapper mapper) 
            : base(repo, mapper)
        {
        }
    }
}
