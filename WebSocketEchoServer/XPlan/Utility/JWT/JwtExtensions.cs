using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Entities;

using System.Text;
using XPlan.Utility.Caches;
using XPlan.Utility.Databases;
using XPlan.Utility.Exceptions;

namespace XPlan.Utility
{
    /// <summary>
    /// JWT 相關設定
    /// </summary>
    public class JwtOptions
    {
        public bool ValidateIssuer { get; set; }            = true;     // 驗證簽發者
        public bool ValidateAudience { get; set; }          = true;     // 驗證接收者
        public bool ValidateLifetime { get; set; }          = true;     // 驗證有效期限
        public bool ValidateIssuerSigningKey { get; set; }  = true;     // 驗證簽章金鑰
        public string Issuer { get; set; }                  = "";       // 簽發者
        public string Audience { get; set; }                = "";       // 接收者
        public string Secret { get; set; }                  = "";       // 秘密金鑰
    }

    public static class JwtExtensions
    {
        /// <summary>
        /// 設定 JWT 認證服務
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer              = jwtOptions.ValidateIssuer,
                            ValidateAudience            = jwtOptions.ValidateAudience,
                            ValidateLifetime            = jwtOptions.ValidateLifetime,
                            ValidateIssuerSigningKey    = jwtOptions.ValidateIssuerSigningKey,
                            ValidIssuer                 = jwtOptions.Issuer,
                            ValidAudience               = jwtOptions.Audience,
                            IssuerSigningKey            = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                        };
                    });

            services.AddSingleton<JwtOptions>(jwtOptions);

            return services;
        }

        /// <summary>
        /// 在 Swagger 中加入 JWT 安全定義
        /// </summary>
        public static IServiceCollection AddJWTSecurity(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // 加入 JWT 認證設定
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description     = "請輸入 JWT Token，格式: Bearer {token}",
                    Name            = "Authorization",
                    In              = ParameterLocation.Header,
                    Type            = SecuritySchemeType.ApiKey,
                    Scheme          = "Bearer",
                    BearerFormat    = "JWT"
                });
            });
            return services;
        }
    }
}
