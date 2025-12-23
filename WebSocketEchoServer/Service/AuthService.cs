using AutoMapper;
using Common.DTO.Login;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using System.Security.Authentication;
using XPlan.Exceptions;
using XPlan.Service;
using XPlan.Utility;
using XPlan.Utility.JWT;

namespace Service
{
    public class AuthService : GenericService<AuthEntity, LoginRequest, LoginResponse, IAuthRepository>, IAuthService
    {
        static private readonly string _salt = "ShowMeTheMoney";
        private readonly JwtOptions _jwtOptions;
        public AuthService(IAuthRepository repo, IMapper mapper, JwtOptions jwtOptions) 
            : base(repo, mapper)
        {
            _jwtOptions = jwtOptions;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var entity = await _repository.GetAsync(request.Account);

                // 對外統一：避免帳號枚舉
                if (entity == null)
                    throw new InvalidCredentialsException();

                var inputHash = Utils.ComputeSha256Hash(request.Password, _salt);
               
                if (!string.Equals(entity.PasswordHash, inputHash, StringComparison.Ordinal))
                    throw new InvalidCredentialsException();

                var token = new JwtGenerator(_jwtOptions.Secret, _jwtOptions.Issuer, _jwtOptions.Audience)
                                            .GenerateToken(entity.Id, entity.Account);

                return new LoginResponse
                {
                    Success = true,
                    Token   = token
                };
            }
            catch (InvalidCredentialsException)
            {
                throw; // 讓上層統一轉 401
            }
            catch (Exception ex)
            {
                // DB / infra 類錯誤統一轉
                throw new ServiceUnavailableException(ex.Message);
            }
        }
    }
}
