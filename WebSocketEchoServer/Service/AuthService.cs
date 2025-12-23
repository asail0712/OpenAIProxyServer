using AutoMapper;
using Common.DTO.Auth;
using Service.DTO.Auth;
using Microsoft.AspNetCore.Mvc;
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
    public class AuthService : GenericService<AuthEntity, IdentityRequest, LoginResponse, IAuthRepository>, IAuthService
    {
        static private readonly string _salt = "ShowMeTheMoney";
        private readonly JwtOptions _jwtOptions;
        public AuthService(IAuthRepository repo, IMapper mapper, JwtOptions jwtOptions) 
            : base(repo, mapper)
        {
            _jwtOptions = jwtOptions;
        }

        public async Task<LoginResponse> Login(IdentityRequest request)
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

        public async Task<bool> CreateIdentity(IdentityRequest request)
        {
            try
            {
                // 1. 檢查輸入參數
                if (string.IsNullOrWhiteSpace(request.Account) ||
                string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new InvalidCredentialsException();
                }
                // 2. 建立新使用者
                var newEntity = new AuthEntity
                {
                    Account         = request.Account,
                    PasswordHash    = Utils.ComputeSha256Hash(request.Password, _salt),
                    CreatedAt       = DateTime.UtcNow,
                    UpdatedAt       = DateTime.UtcNow
                };
                var inserted = await _repository.InsertAsync(newEntity);
                return inserted != null;
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

        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                // 1. 檢查輸入參數
                if (string.IsNullOrWhiteSpace(request.Account) ||
                string.IsNullOrWhiteSpace(request.OldPassword) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    throw new InvalidCredentialsException();
                }

                // 2. 取得使用者資料
                var staffData = await _repository.GetAsync(request.Account);

                if (staffData == null)
                {
                    throw new InvalidCredentialsException();
                }

                // 3. 驗證舊密碼
                if (staffData.PasswordHash != Utils.ComputeSha256Hash(request.OldPassword, _salt))
                {
                    throw new InvalidCredentialsException();
                }

                // 4. 更新新密碼
                string newPwHash = Utils.ComputeSha256Hash(request.NewPassword, _salt);

                return await _repository.ChangePassword(staffData.Account, newPwHash);
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
