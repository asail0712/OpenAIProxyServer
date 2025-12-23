using DataAccess;
using DataAccess.Interface;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;

namespace OpenAIProxyService.Extension
{
    public static class AddServiceExtension
    {
        static public void AddDataAccesses(this IServiceCollection services)
        {
            services.AddScoped<IAuthDataAccess, AuthDataAccess>();
        }

        static public void AddRepositorys(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
        }

        static public void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
