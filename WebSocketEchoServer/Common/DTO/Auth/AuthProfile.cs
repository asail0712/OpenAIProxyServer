using AutoMapper;

namespace Common.DTO.Auth
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
