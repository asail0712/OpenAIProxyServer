using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using DataAccess.Interface;
using Repository.Interface;

using XPlan.Repository;
using XPlan.Utility.Caches;
using Common.DTO.Login;

namespace Repository
{
    public class LoginRepository : GenericRepository<LoginEntity, ILoginDataAccess>, ILoginRepository
    {
        public LoginRepository(ILoginDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings) 
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }
    }
}
