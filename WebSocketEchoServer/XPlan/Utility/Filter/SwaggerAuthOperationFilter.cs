using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace XPlan.Utility.Filter
{
    public class SwaggerAuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 只要 method 或 controller 任一處有 [AllowAnonymous]，就不需要鎖頭
            var allowAnonymous =
                context.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>(true) != null ||
                context.MethodInfo.DeclaringType?.GetCustomAttribute<AllowAnonymousAttribute>(true) != null;

            if (allowAnonymous)
                return;

            // 有 [Authorize] 或你是「全域預設都要授權」的做法時：我們直接讓它顯示鎖頭
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type    = ReferenceType.SecurityScheme,
                        Id      = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        }
    }
}
