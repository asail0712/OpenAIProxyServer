using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using DataAccess.Interface;
using Repository.Interface;

using XPlan.Repository;
using XPlan.Utility.Caches;
using Common.DTO.Auth;

namespace Repository
{
    public class AuthRepository : GenericRepository<AuthEntity, IAuthDataAccess>, IAuthRepository
    {
        public AuthRepository(IAuthDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings) 
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }
    }
}
