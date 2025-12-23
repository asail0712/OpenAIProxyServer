using AutoMapper;

namespace Common.DTO.Login
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            //CreateMap<LoginRequest, AuthEntity>();
            //CreateMap<AuthEntity, LoginResponse>();
            CreateMap<AuthEntity, AuthDocument>();
            CreateMap<AuthDocument, AuthEntity>();
        }
    }

}
