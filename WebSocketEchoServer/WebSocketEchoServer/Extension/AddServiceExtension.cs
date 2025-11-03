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
            services.AddScoped<ILoginDataAccess, LoginDataAccess>();
        }

        static public void AddRepositorys(this IServiceCollection services)
        {
            services.AddScoped<ILoginRepository, LoginRepository>();
        }

        static public void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
        }
    }
}
